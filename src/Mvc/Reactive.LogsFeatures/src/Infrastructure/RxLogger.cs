using EquipApps.Mvc.Reactive.LogsFeatures.Models;
using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    public class RxLogger : ILogger
    {
        private readonly string categoryName;
        private readonly ILogService logService;

        public RxLogger(string categoryName, ILogService logService)
        {
            this.categoryName = categoryName;
            this.logService = logService;
        }

        internal IExternalScopeProvider ScopeProvider { get; set; } = RxLoggerScopeProvider.Instance;

        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state) ?? NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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

        private LogEntryLevel Transformer(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return LogEntryLevel.dbug;
                case LogLevel.Information:
                    return LogEntryLevel.info;

                case LogLevel.Warning:
                    return LogEntryLevel.warn;

                case LogLevel.Error:
                case LogLevel.Critical:
                case LogLevel.None:
                default:
                    return LogEntryLevel.fail;
            }


        }
    }
}
