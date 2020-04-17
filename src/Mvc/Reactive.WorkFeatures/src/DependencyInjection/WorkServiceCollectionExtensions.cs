using EquipApps.WorkBench.ViewModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WorkServiceCollectionExtensions 
    {
        public static void AddWorker(this IServiceCollection services)
        {
            //-- Middleware
            //services.AddSingleton<ViewMiddleware>();

            //-- Extentions
            //services.AddTransientFeatureProvider<ViewFeatureProvider>();
            //services.AddTransientActionInvokerProvider<ViewActionInvokerProvider>();

            //-- Service
            //services.AddSingleton<IActionService, ActionService>();

            //-- Viewer
            services.AddTransient<WorkViewer>();
        }
    }
}
