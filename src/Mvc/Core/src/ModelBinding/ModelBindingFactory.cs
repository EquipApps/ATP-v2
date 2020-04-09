using EquipApps.Mvc;
using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    public class ModelBindingFactory : IModelBindingFactory
    {
        private readonly IReadOnlyList<IBinderProvider> _providers;
        private readonly ILogger<ModelBindingFactory> _logger;

        private readonly IModelMetadataProvider _metadataProvider;
        private readonly IPropertyProvider _propertyProvider;

        public ModelBindingFactory(
            IOptions<MvcOptions> option,
            ILogger<ModelBindingFactory> logger,

            IModelMetadataProvider metadataProvider,
            IPropertyProvider propertyProvide)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            _propertyProvider = propertyProvide ?? throw new ArgumentNullException(nameof(propertyProvide));
            _providers = option?.Value.BindingProviders ?? throw new ArgumentNullException(nameof(option));

            if (_providers.Count == 0)
            {
                throw new InvalidOperationException();
            }

            logger.LogTrace("ctr");
        }

        public IModelBinder Create(IBindingModel bindingModel, BindingInfo bindingInfo)
        {
            if (bindingModel == null)
            {
                throw new InvalidOperationException(nameof(bindingModel));
            }

            if (bindingInfo == null)
            {
                throw new InvalidOperationException(nameof(bindingModel));
            }

            var providerContext = new DefaultModelBinderProviderContext(
                this,
                bindingModel,
                bindingInfo);


            IModelBinder result = null;

            for (var i = 0; i < _providers.Count; i++)
            {
                var provider = _providers[i];

                result = provider.GetBinder(providerContext);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }


        private class DefaultModelBinderProviderContext : BinderProviderContext
        {
            private ModelBindingFactory _factory;

            public DefaultModelBinderProviderContext(ModelBindingFactory factory, IBindingModel bindableElement, BindingInfo bindingInfo)
            {
                _factory = factory;
                BindingModel = bindableElement;
                BindingInfo = bindingInfo;
            }

            public override IBindingModel BindingModel
            {
                get;
            }

            public override BindingInfo BindingInfo
            {
                get;
            }



            //---
            public override IPropertyProvider PropertyProvider => _factory._propertyProvider;

            //---
            public override IModelMetadataProvider MetadataProvider => _factory._metadataProvider;

            //---
            public override IModelBinder CreateBinder(ModelMetadata property)
            {
                //TODO: До делать когданибудь
                throw new NotImplementedException();
            }
        }
    }
}
