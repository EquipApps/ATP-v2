using EquipApps.Mvc.Internal;
using EquipApps.Mvc.Services;
using EquipApps.Mvc.Viewers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ViewerServiceCollectionExtensions 
    {
        public static void AddViewer(this IServiceCollection services)
        {
            //-- Extentions
            services.AddTransientFeatureProvider<ViewFeatureProvider>();

            //-- Регистрация сервисов
            services.AddSingleton<IActionService, ActionService>();


            //-- Viewers
            services.AddTransient<ActionsViewer>();

        }
    }
}
