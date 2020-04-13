using EquipApps.Mvc.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace EquipApps.Mvc.Infrastructure
{
    public class ViewLogger : ILogger
    {
        private readonly string categoryName;
        private readonly ILogService logService;

        public ViewLogger(string categoryName, ILogService  logService)
        {
            this.categoryName = categoryName;
            this.logService = logService;
        }

        internal IExternalScopeProvider ScopeProvider { get; set; } = ViewLoggerScopeProvider.Instance;

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state) ?? NullScope.Instance;
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                logService.EnqueueEntry(new LogEntry
                {
                    Context = categoryName,
                    Message = message,
                    Level = Transformer(logLevel),
                    Time = DateTimeOffset.Now,

                    Scope = GetScopeInformation(),
                });
            }
        }

        private string GetScopeInformation()
        {
            var scopeProvider = ScopeProvider;
            if (scopeProvider != null)//(Options.IncludeScopes && scopeProvider != null)
            {
                var stringBuilder = new StringBuilder();

                var initialLength = stringBuilder.Length;

                scopeProvider.ForEachScope((scope, state) =>
                {
                    var (builder, length) = state;
                    var first = length == builder.Length;
                    //builder.Append(first ? "=> " : " => ");
                    builder.Append(scope);
                }, (stringBuilder, initialLength));

                //if (stringBuilder.Length > initialLength)
                //{
                //    stringBuilder.Insert(initialLength, _messagePadding);
                //    stringBuilder.AppendLine();
                //}

                return stringBuilder.ToString();
            }

            return null;
        }

        private LogLevel Transformer(Microsoft.Extensions.Logging.LogLevel level)
        {
            switch (level)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return LogLevel.dbug;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return LogLevel.info;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return LogLevel.warn;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                case Microsoft.Extensions.Logging.LogLevel.None:
                default:
                    return LogLevel.fail;
            }


        }
    }
}
