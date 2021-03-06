﻿using EquipApps.Mvc.ApplicationModels.Сustomization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels.Infrastructure
{
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
