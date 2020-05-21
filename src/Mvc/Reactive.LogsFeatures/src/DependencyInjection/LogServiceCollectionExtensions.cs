using EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure;
using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogServiceCollectionExtensions 
    {
        public static void AddMvcLogs(this IServiceCollection services)
        {
            //-- Middleware
            services.AddSingleton<LogsMiddleware>();

            //-- Extentions
            services.AddTransientFeatureProvider<LogsFeatureProvider>();

            //-- Extention            
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ILoggerProvider, RxLoggerProvider>();
        }
    }
}
