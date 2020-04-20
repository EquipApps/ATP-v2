using EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure;
using EquipApps.Mvc.Reactive.WorkFeatures.Services;
using EquipApps.Mvc.Reactive.WorkFeatures.Viewers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WorkServiceCollectionExtensions 
    {
        public static void AddWorker(this IServiceCollection services)
        {
            //-- Middleware
            services.AddSingleton<RuntimeMiddleware>();

            //-- Extentions


            //-- Service
            services.AddSingleton<IRuntimeService>(x => x.GetService<RuntimeMiddleware>());


            //-- Viewer
            services.AddTransient<WorkViewer>();
        }
    }
}
