using EquipApps.Mvc.Runtime;
using EquipApps.WorkBench.ViewModels;

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
