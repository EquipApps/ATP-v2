using EquipApps.Mvc.Abstractions;
using EquipApps.WorkBench.Services;
using EquipApps.WorkBench.ViewModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WbServiceCollectionExtensions
    {
        public static void AddWb(this IServiceCollection services)
        {
            // ----------------------------------------------------------------------------------------
            // CORE()
            //
            // 
            services.AddSingleton<IActionService,   ActionService>();
            services.AddSingleton<ILogEntryService, LogEntryService>();

            
            services.AddSingleton<ActionViewModel>();


            // ----------------------------------------------------------------------------------------
            // Action Descriptor Factory
            // Singleton
            // ( Переопределяет провайдер по умолчанию )
            services.AddSingleton<IActionDescripterFactory>(serviceProvider =>
            {
                return serviceProvider.GetService<IActionService>() as ActionService;
            });
        }
    }
}
