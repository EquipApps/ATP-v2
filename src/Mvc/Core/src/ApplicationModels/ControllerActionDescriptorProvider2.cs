using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class ControllerActionDescriptorProvider2 : IActionDescriptorProvider
    {
        private ApplicationPartManager _partManager;
        private ApplicationModelFactory _applicationModelFactory;
        private readonly IBindingFactory _modelBinderFactory;

        public ControllerActionDescriptorProvider2(ApplicationPartManager partManager,
                                                   ApplicationModelFactory applicationModelFactory)
        {
            if (partManager == null)
            {
                throw new ArgumentNullException(nameof(partManager));
            }

            if (applicationModelFactory == null)
            {
                throw new ArgumentNullException(nameof(applicationModelFactory));
            }

            _partManager = partManager;
            _applicationModelFactory = applicationModelFactory;
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
        }

        internal IEnumerable<ControllerActionDescriptor> GetDescriptors()
        {
            var controllerTypes = GetControllerTypes();
            var application     = _applicationModelFactory.CreateApplicationModel(controllerTypes);

            //------------------

            foreach (var controller in application.Controllers)
            {

            }

            //------------------

            

            return null;
        }








        private IEnumerable<TypeInfo> GetControllerTypes()
        {
            var feature = new ControllerFeature();
            _partManager.PopulateFeature(feature);
            return feature.Controllers;
        }

        


    }
}
