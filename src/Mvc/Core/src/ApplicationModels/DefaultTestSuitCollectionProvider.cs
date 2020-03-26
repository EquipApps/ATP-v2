using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
using EquipApps.Mvc.Objects;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EquipApps.Mvc.ApplicationModels
{
    public class DefaultTestSuitCollectionProvider : TestSuitCollectionProvider
    {
        private ApplicationPartManager _partManager;
        private ApplicationModelFactory _applicationModelFactory;

        public DefaultTestSuitCollectionProvider(ApplicationPartManager partManager,
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


            Initializer();
        }

        private void Initializer()
        {
            var controllerTypes = GetControllerTypes();
            var application     = _applicationModelFactory.CreateApplicationModel(controllerTypes);
        }

        public override TestSuitCollection TestSuits => throw new NotImplementedException();

        public override IChangeToken GetChangeToken()
        {
            throw new NotImplementedException();
        }


        private IEnumerable<TypeInfo> GetControllerTypes()
        {
            var feature = new ControllerFeature();
            _partManager.PopulateFeature(feature);

            return feature.Controllers;
        }





    }
}
