﻿using EquipApps.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private IServiceProvider _serviceProvider;

        public MvcOptionsSetup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Configure(MvcOptions options)
        {
            ConfigureBindingProviders(options);

            // Set up metadata providers
            ConfigureAdditionalModelMetadataDetailsProviders(options.ModelMetadataDetailsProviders);
        }

        internal static void ConfigureAdditionalModelMetadataDetailsProviders(IList<IMetadataDetailsProvider> modelMetadataDetailsProviders)
        {
            // Don't bind the Type class by default as it's expensive. A user can override this behavior
            // by altering the collection of providers.
            modelMetadataDetailsProviders.Add(new ExcludeBindingMetadataProvider(typeof(Type)));

            modelMetadataDetailsProviders.Add(new DefaultBindingMetadataProvider());
            modelMetadataDetailsProviders.Add(new DefaultValidationMetadataProvider());

            // TODO: Создават привязк если есть интерфейс! IModelExpected<>
            //modelMetadataDetailsProviders.Add(new BindingSourceMetadataProvider(typeof(CancellationToken), BindingSource.Special));
        }

        private void ConfigureBindingProviders(MvcOptions options)
        {
            options.BindingProviders.Add(new ModelProviderModelBinderProvider(_serviceProvider));
            options.BindingProviders.Add(new DataContextModelBinderProvider());
            options.BindingProviders.Add(new DataTextModelBinderProvider());
        }
    }
}
