using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using EquipApps.Testing.Features;
using System;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    public class LogsFeatureProvider : IFeatureProvider
    {
        private ILogService _logService;

        public LogsFeatureProvider(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }


        public int Order => 0;

        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            context.Collection.Set(new LogsFeature(_logService));
        }

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //---
        }
    }
}
