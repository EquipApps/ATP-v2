using EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure;
using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using EquipApps.Mvc.Reactive.ViewFeatures.Viewers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ViewerServiceCollectionExtensions 
    {
        public static void AddMvcView(this IServiceCollection services)
        {
            //-- Middleware
            services.AddSingleton<ViewMiddleware>();

            //-- Extentions
            services.AddTransientFeatureProvider<ViewFeatureProvider>();
            services.AddTransientActionInvokerProvider<ViewActionInvokerProvider>();

            //-- Service
            services.AddSingleton<IActionService, ActionService>();

            //-- Service
            services.AddTransient<ActionsViewer>();
        }
    }
}
