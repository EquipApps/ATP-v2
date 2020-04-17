using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using EquipApps.Testing.Features;
using System;
using System.Diagnostics;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure
{
    /// <summary>
    /// Расширение.
    /// Регистрирует <see cref="ViewFeature"/>.
    /// </summary>
    public class ViewFeatureProvider : IFeatureProvider
    {
        private IActionService _actionService;

        public ViewFeatureProvider(IActionService actionService)
        {
            _actionService = actionService ?? throw new ArgumentNullException(nameof(actionService));
        }

        public int Order => 0;


        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            //-- Поиск IMvcFeature
            var mvcFechure = context.Collection.Get<IMvcFeature>();

            //--
            Debug.Assert(mvcFechure != null, "IMvcFeature не найден");

            //-- Создаем фичу!
            var viewFeature = new ViewFeature(_actionService, mvcFechure);

            //-- 
            context.Collection.Set(viewFeature);
        }
        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //---
        }
    }
}
