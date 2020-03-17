using EquipApps.Mvc.Runtime;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding.Binders;
using NLib.AtpNetCore.Testing.Mvc.Runtime;
using NLib.AtpNetCore.Testing.Mvc.Runtime.Internal;
using System;

namespace NLib.AtpNetCore.Testing.Mvc
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
            ConfigureRuntimeStates(options);
        }

        private static void ConfigureRuntimeStates(MvcOption options)
        {

            options.RuntimeStates.Add(RuntimeStateId.Start, new Runtime_State1_Reset());

            options.RuntimeStates.Add(RuntimeStateId.Invoke, new Runtime_State2_Invoke());
            options.RuntimeStates.Add(RuntimeStateId.Invoke + 100, new Runtime_State3_Pause());


            options.RuntimeStates.Add(RuntimeStateId.Move - 100, new Runtime_State4_RepeatOnce());
            options.RuntimeStates.Add(RuntimeStateId.Move, new Runtime_State5_Move());

            options.RuntimeStates.Add(RuntimeStateId.End - 100, new Runtime_State6_Repeat());
            options.RuntimeStates.Add(RuntimeStateId.End, new Runtime_State7_End());
        }

        private void ConfigureBindingProviders(MvcOption options)
        {
            options.BindingProviders.Add(new ModelProviderModelBinderProvider(_serviceProvider));
            options.BindingProviders.Add(new DataContextModelBinderProvider());
            options.BindingProviders.Add(new DataTextModelBinderProvider());
        }
    }
}
