using EquipApps.WorkBench.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Services
{
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();



        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }


    /// <summary>
    /// Scope provider that does nothing.
    /// </summary>
    internal class NullExternalScopeProvider : IExternalScopeProvider
    {
        private NullExternalScopeProvider()
        {
        }

        private List<Scope> scopes = new List<Scope>();


        /// <summary>
        /// Returns a cached instance of <see cref="NullExternalScopeProvider"/>.
        /// </summary>
        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();

        /// <inheritdoc />
        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
            foreach (var scope in scopes)
            {
                callback(scope.Obj, state);
            }
        }

        /// <inheritdoc />
        IDisposable IExternalScopeProvider.Push(object state)
        {
            return new Scope(state, this);
        }

        public class Scope : IDisposable
        {
            private NullExternalScopeProvider provider;
            public object Obj;

            public Scope(object state, NullExternalScopeProvider provider)
            {
                this.provider = provider;
                Obj = state;

                provider.scopes.Add(this);
            }

            public void Dispose()
            {
                provider.scopes.Remove(this);
            }
        }
    }




    public class LogEntryLogger : ILogger
    {
        private string categoryName;
        private ILogEntryService entryService;

        public LogEntryLogger(string categoryName, ILogEntryService entryService)
        {
            this.categoryName = categoryName;
            this.entryService = entryService;
        }


        internal IExternalScopeProvider ScopeProvider { get; set; } = NullExternalScopeProvider.Instance;

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
                WriteMessage(logLevel, categoryName, eventId.Id, message, exception);
            }
        }

        private void WriteMessage(Microsoft.Extensions.Logging.LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var logBuilder = new StringBuilder();


            var logLevelString = GetLogLevelString(logLevel);
            // category and event id
            //logBuilder.Append(_loglevelPadding);
            //logBuilder.Append(logName);
            //logBuilder.Append("[");
            //logBuilder.Append(eventId);
            //logBuilder.AppendLine("]");

            // scope information
            //GetScopeInformation(logBuilder);

            if (!string.IsNullOrEmpty(message))
            {
                // message
                logBuilder.Append(_messagePadding);

                var len = logBuilder.Length;
                logBuilder.AppendLine(message);
                logBuilder.Replace(Environment.NewLine, _newLineWithMessagePadding, len, message.Length);
            }

            if (exception != null)
            {
                // exception message
                logBuilder.AppendLine(exception.ToString());
            }

            entryService.EnqueueEntry(new LogEntry
            {
                Context = logName,
                Message = message,//logBuilder.ToString(),
                LogLevel = Transformer(logLevel),
                Time = DateTimeOffset.Now,

                Scope = GetScopeInformation(),

            });
        }

        private static string GetLogLevelString(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return "trce";
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return "dbug";
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return "info";
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return "warn";
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return "fail";
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return "crit";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        private static readonly string _loglevelPadding = ": ";
        private static readonly string _messagePadding;
        private static readonly string _newLineWithMessagePadding;


        private StringBuilder scopeStringBuilder = new StringBuilder();

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

        private void GetScopeInformation(StringBuilder stringBuilder)
        {
            var scopeProvider = ScopeProvider;
            if (scopeProvider != null)//(Options.IncludeScopes && scopeProvider != null)
            {
                var initialLength = stringBuilder.Length;

                scopeProvider.ForEachScope((scope, state) =>
                {
                    var (builder, length) = state;
                    var first = length == builder.Length;
                    builder.Append(first ? "=> " : " => ").Append(scope);
                }, (stringBuilder, initialLength));

                if (stringBuilder.Length > initialLength)
                {
                    stringBuilder.Insert(initialLength, _messagePadding);
                    stringBuilder.AppendLine();
                }
            }
        }

        private Models.LogLevel Transformer(Microsoft.Extensions.Logging.LogLevel level)
        {
            switch (level)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return Models.LogLevel.dbug;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return Models.LogLevel.info;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return Models.LogLevel.warn;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                case Microsoft.Extensions.Logging.LogLevel.None:
                default:
                    return Models.LogLevel.fail;
            }


        }
    }
}
