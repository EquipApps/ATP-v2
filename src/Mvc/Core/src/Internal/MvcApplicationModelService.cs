using EquipApps.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Mvc.Internal
{
    //TODO: Реализовать буферизацию через настройки Mvc Options (Это может пригодиться когда подключаются или отключаются динамические библеотеки)
    public class MvcApplicationModelService : IApplicationModelService
    {
        private readonly IApplicationModelProvider[] _providers;
        private readonly ILogger<MvcApplicationModelService> logger;

        ApplicationModel applicationModel;


        public MvcApplicationModelService(
            ILogger<MvcApplicationModelService> logger,
            IEnumerable<IApplicationModelProvider> applicationModelProviders)
        {
            if (applicationModelProviders == null)
            {
                throw new ArgumentNullException(nameof(applicationModelProviders));
            }

            _providers = applicationModelProviders
              .OrderBy(p => p.Order)
              .ToArray();

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.logger.LogTrace("ctr");
        }

        public ApplicationModel GetApplicationModel()
        {
            logger.LogTrace(nameof(GetApplicationModel));

            if (applicationModel == null)
            {
                BuildModel();
            }
            return applicationModel;
        }

        private void BuildModel()
        {
            logger.LogTrace(nameof(BuildModel));

            if (_providers.Length == 0)
            {
                throw new InvalidOperationException("ApplicationModelProviderAreRequired");
            }

            var context = new ApplicationModelContext();

            //--- Создание

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnProvidersExecuting(context);
            }

            for (var i = _providers.Length - 1; i >= 0; i--)
            {
                _providers[i].OnProvidersExecuted(context);
            }

            applicationModel = context.Result;
        }
    }
}
