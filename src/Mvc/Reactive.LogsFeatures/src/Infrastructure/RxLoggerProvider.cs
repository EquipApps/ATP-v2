using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using Microsoft.Extensions.Logging;
using System;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    public class RxLoggerProvider : ILoggerProvider
    {
        private readonly ILogService logEntryService;

        public RxLoggerProvider(ILogService logEntryService)
        {
            this.logEntryService = logEntryService ?? throw new ArgumentNullException(nameof(logEntryService));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RxLogger(categoryName, logEntryService);
        }

        public void Dispose()
        {
            //--
        }
    }
}
