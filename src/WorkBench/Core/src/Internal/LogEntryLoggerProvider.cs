using Microsoft.Extensions.Logging;
using System;

namespace EquipApps.WorkBench.Services
{
    public class LogEntryLoggerProvider : ILoggerProvider
    {
        private readonly ILogEntryService logEntryService;

        public LogEntryLoggerProvider(ILogEntryService logEntryService)
        {
            this.logEntryService = logEntryService ?? throw new ArgumentNullException(nameof(logEntryService));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LogEntryLogger(categoryName, logEntryService);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
