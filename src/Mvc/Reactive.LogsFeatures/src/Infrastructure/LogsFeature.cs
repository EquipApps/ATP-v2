using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using System;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    public class LogsFeature : IDisposable
    {
        private readonly ILogService logService;

        public LogsFeature(ILogService logService)
        {
            this.logService = logService;
        }

        public void Dispose()
        {
            logService.Clean();
        }
    }
}
