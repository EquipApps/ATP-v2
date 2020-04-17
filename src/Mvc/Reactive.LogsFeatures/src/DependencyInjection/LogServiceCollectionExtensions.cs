using EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure;
using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using EquipApps.Mvc.Reactive.LogsFeatures.Viewers;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogServiceCollectionExtensions 
    {
        public static void AddLogs(this IServiceCollection services)
        {
            //-- Middleware
            services.AddSingleton<LogsMiddleware>();

            //-- Extention            
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ILoggerProvider, RxLoggerProvider>();

            //-- Viewers            
            services.AddTransient<LogViewer>();
        }
    }
}
