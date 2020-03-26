using EquipApps.Mvc;
using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Metadata;
using EquipApps.Mvc.ModelBinding.Property;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    public class BindingFactory : IBindingFactory
    {
        private readonly IReadOnlyList<IBinderProvider> _providers;
        private readonly ILogger<BindingFactory> _logger;

        private readonly IMetadataProvider _metadataProvider;
        private readonly IPropertyProvider _propertyProvider;

        public BindingFactory(
            IOptions<MvcOptions> option,
            ILogger<BindingFactory> logger,

            IMetadataProvider metadataProvider,
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

        public IBinder Create(IBindingModel bindingModel, BindingInfo bindingInfo)
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


            IBinder result = null;

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
            private BindingFactory _factory;

            public DefaultModelBinderProviderContext(BindingFactory factory, IBindingModel bindableElement, BindingInfo bindingInfo)
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
            public override IMetadataProvider MetadataProvider => _factory._metadataProvider;

            //---
            public override IBinder CreateBinder(ModelMetadata property)
            {
                //TODO: До делать когданибудь
                throw new NotImplementedException();
            }
        }
    }
}
