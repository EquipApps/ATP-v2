using EquipApps.Mvc.Services;
using Microsoft.Extensions.Logging;
using System;

namespace EquipApps.Mvc.Infrastructure
{
    public class ViewLoggerProvider : ILoggerProvider
    {
        private readonly ILogService logEntryService;

        public ViewLoggerProvider(ILogService logEntryService)
        {
            this.logEntryService = logEntryService ?? throw new ArgumentNullException(nameof(logEntryService));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ViewLogger(categoryName, logEntryService);
        }

        public void Dispose()
        {
            //--
        }
    }
}
