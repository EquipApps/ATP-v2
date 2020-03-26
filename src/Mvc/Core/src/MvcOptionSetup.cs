using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc.ModelBinding.Binders;
using System;

namespace EquipApps.Mvc
{
    public class MvcOptionSetup : IConfigureOptions<MvcOption>
    {
        private IServiceProvider _serviceProvider;

        public MvcOptionSetup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Configure(MvcOption options)
        {
            ConfigureBindingProviders(options);
        }


        private void ConfigureBindingProviders(MvcOption options)
        {
            options.BindingProviders.Add(new ModelProviderModelBinderProvider(_serviceProvider));
            options.BindingProviders.Add(new DataContextModelBinderProvider());
            options.BindingProviders.Add(new DataTextModelBinderProvider());
        }
    }
}
