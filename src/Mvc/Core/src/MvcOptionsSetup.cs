using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc.ModelBinding.Binders;
using System;

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
        }


        private void ConfigureBindingProviders(MvcOptions options)
        {
            options.BindingProviders.Add(new ModelProviderModelBinderProvider(_serviceProvider));
            options.BindingProviders.Add(new DataContextModelBinderProvider());
            options.BindingProviders.Add(new DataTextModelBinderProvider());
        }
    }
}
