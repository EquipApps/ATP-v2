using EquipApps.Mvc.Services;
using EquipApps.Testing.Features;
using Microsoft.Extensions.Logging;
using System;

namespace EquipApps.Mvc.Internal
{
    /// <summary>
    /// Расширение.
    /// Регистрирует <see cref="ViewFeature"/>.
    /// </summary>
    public class ViewFeatureProvider : IFeatureProvider
    {
        private ILogger<ViewFeatureProvider> logger;
        private IActionService _actionService;

        public ViewFeatureProvider(ILoggerFactory loggerFactory,
                                   IActionService actionService)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            if (actionService == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ViewFeatureProvider>();

            _actionService = actionService;
        }

        public int Order => 0;

        
        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            logger.LogTrace(nameof(OnProvidersExecuted));

            //-- Поиск IMvcFeature
            var mvcFechure = context.Collection.Get<IMvcFeature>();
            if (mvcFechure == null)
            {
                logger.LogError("IMvcFeature не найден");
                return;
            }

            //-- Создаем фичу!
            var viewFeature = new ViewFeature(_actionService, mvcFechure);

            //-- 
            context.Collection.Set(viewFeature);
        }
        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            logger.LogTrace(nameof(OnProvidersExecuting));
        }
    }
}
