using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    //TODO: Реализовать буферизацию через настройки Mvc Options (Это может пригодиться когда подключаются или отключаются динамические библеотеки)
    public class ApplicationModelFactory
    {
        private readonly IApplicationModelProvider[] _applicationModelProviders;
        private readonly IList<IApplicationModelConvention> _conventions;

        public ApplicationModelFactory(
            IEnumerable<IApplicationModelProvider> applicationModelProviders,
            IOptions<MvcOptions> options)
        {
            if (applicationModelProviders == null)
            {
                throw new ArgumentNullException(nameof(applicationModelProviders));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _applicationModelProviders = applicationModelProviders.OrderBy(p => p.Order).ToArray();
            _conventions = options.Value.Conventions;
        }

        public ApplicationModel GetApplicationModel()
        {
            return CreateApplicationModel(new TypeInfo[0]);
        }

        public ApplicationModel CreateApplicationModel(IEnumerable<TypeInfo> controllerTypes)
        {
            if (controllerTypes == null)
            {
                throw new ArgumentNullException(nameof(controllerTypes));
            }

            var context = new ApplicationModelProviderContext(controllerTypes);

            for (var i = 0; i < _applicationModelProviders.Length; i++)
            {
                _applicationModelProviders[i].OnProvidersExecuting(context);
            }

            for (var i = _applicationModelProviders.Length - 1; i >= 0; i--)
            {
                _applicationModelProviders[i].OnProvidersExecuted(context);
            }

            ApplicationModelConventions.ApplyConventions(context.Result, _conventions);

            return context.Result;
        }
    }
}
