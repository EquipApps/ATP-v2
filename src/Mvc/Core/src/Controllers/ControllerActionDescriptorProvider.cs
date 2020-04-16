using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels.Infrastructure;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class ControllerActionDescriptorProvider : IActionDescriptorProvider
    {
        private ApplicationPartManager _partManager;
        private ApplicationModelFactory _applicationModelFactory;
        private ControllerActionDescriptorBuilder _controllerActionDescriptorBuilder;

        public ControllerActionDescriptorProvider(ApplicationPartManager partManager,
                                                  ApplicationModelFactory applicationModelFactory,
                                                  ControllerActionDescriptorBuilder controllerActionDescriptorBuilder)
        {
            if (partManager == null)
            {
                throw new ArgumentNullException(nameof(partManager));
            }

            if (applicationModelFactory == null)
            {
                throw new ArgumentNullException(nameof(applicationModelFactory));
            }

            if (controllerActionDescriptorBuilder == null)
            {
                throw new ArgumentNullException(nameof(controllerActionDescriptorBuilder));
            }

            _partManager = partManager;
            _applicationModelFactory = applicationModelFactory;
            _controllerActionDescriptorBuilder = controllerActionDescriptorBuilder;
        }

        public int Order => -1000;

        /// <inheritdoc />
        public void OnProvidersExecuting(ActionDescriptorProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var descriptor in GetDescriptors())
            {
                context.Results.Add(descriptor);
            }
        }

        /// <inheritdoc />
        public void OnProvidersExecuted(ActionDescriptorProviderContext context)
        {
            // After all of the providers have run, we need to provide a 'null' for each all of route values that
            // participate in action selection.
            //
            // This is important for scenarios like Razor Pages, that use the 'page' route value. An action that
            // uses 'page' shouldn't match when 'action' is set, and an action that uses 'action' shouldn't match when
            // 'page is specified.
            //
            // Or for another example, consider areas. A controller that's not in an area needs a 'null' value for
            // area so it can't match when the route produces an 'area' value.
            var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < context.Results.Count; i++)
            {
                var action = context.Results[i];
                foreach (var key in action.RouteValues.Keys)
                {
                    keys.Add(key);
                }
            }
            for (var i = 0; i < context.Results.Count; i++)
            {
                var action = context.Results[i];
                foreach (var key in keys)
                {
                    if (!action.RouteValues.ContainsKey(key))
                    {
                        action.RouteValues.Add(key, null);
                    }
                }
            }


            keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < context.Results.Count; i++)
            {
                var action = context.Results[i];
                foreach (var key in action.OrderValues.Keys)
                {
                    keys.Add(key);
                }
            }
            for (var i = 0; i < context.Results.Count; i++)
            {
                var action = context.Results[i];
                foreach (var key in keys)
                {
                    if (!action.OrderValues.ContainsKey(key))
                    {
                        action.OrderValues.Add(key, null);
                    }
                }
            }
        }


        internal IEnumerable<ControllerActionDescriptor> GetDescriptors()
        {
            var controllerTypes = GetControllerTypes();
            var application = _applicationModelFactory.CreateApplicationModel(controllerTypes);
            return _controllerActionDescriptorBuilder.Builder(application);
        }

        private IEnumerable<TypeInfo> GetControllerTypes()
        {
            var feature = new ControllerFeature();
            _partManager.PopulateFeature(feature);
            return feature.Controllers;
        }
    }
}
