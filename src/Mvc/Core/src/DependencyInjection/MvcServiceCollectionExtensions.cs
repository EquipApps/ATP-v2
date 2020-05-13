using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ApplicationModels.Infrastructure;
using EquipApps.Mvc.ApplicationModels.Сustomization;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
using EquipApps.Mvc.Infrastructure;
using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Metadata;
using EquipApps.Mvc.ModelBinding.Property;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.Properties;
using System;
using System.Linq;
using System.Reflection;

namespace NLib.AtpNetCore.Testing
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddMvc(this IServiceCollection services)
        {
            var manager = GetApplicationPartManager(services);
            ConfigureApplicationFeatureProviders(manager);

            ConfigureDefaultServices(services);

        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            // ----------------------------------------------------------------------------------------
            // IFeatureProvider
            //      MvcFeatureProvider
            //      Action Descriptor Collection Provider
            // 
            // 
            services.AddTransientFeatureProvider<MvcFeatureProvider>();
            services.AddSingleton<IActionDescriptorCollectionProvider, DefaultActionDescriptorCollectionProvider>();
            //
            // ----------------------------------------------------------------------------------------
            //  Controller ActionDescriptor Provider
            //  Infrostructure
            // 
            services.AddTransientActionDescriptorProvider<ControllerActionDescriptorProvider>();
            services.AddTransient<ControllerActionDescriptorBuilder>();
            //
            //  Application Model Factory           
            //
            services.AddSingleton<ApplicationModelFactory>();
            services.AddTransient<IApplicationModelProvider, DefaultApplicationModelProvider>();
            //
            // ----------------------------------------------------------------------------------------
            







            // ----------------------------------------------------------------------------------------
            // IActionInvokerProvider
            //      ControllerActionInvokerProvider
            // 
            //            

            services.AddTransientActionInvokerProvider<ControllerActionInvokerProvider>();
            services.AddSingleton<ControllerActionInvokerCache>();







            //
            // ModelBinding, Validation
            //
            // The DefaultModelMetadataProvider does significant caching and should be a singleton.
            services.AddSingleton<IModelMetadataProvider, DefaultModelMetadataProvider>();
            services.TryAdd(ServiceDescriptor.Transient<ICompositeMetadataDetailsProvider>(s =>
            {
                var options = s.GetRequiredService<IOptions<MvcOptions>>().Value;
                return new DefaultCompositeMetadataDetailsProvider(options.ModelMetadataDetailsProviders);
            }));













           



            // ----------------------------------------------------------------------------------------

            services.TryAddEnumerable(
               ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcOptionsSetup>());



            services.AddSingleton<IPropertyProvider, PropertyProvider>();








            services.AddSingleton<IModelBinderFactory, ModelBinderFactory>();

        }


        #region ApplicationPartManager

        private static ApplicationPartManager GetApplicationPartManager(IServiceCollection services)
        {
            var manager = GetServiceFromCollection<ApplicationPartManager>(services);

            if (manager == null)
            {
                manager = new ApplicationPartManager();
                services.TryAddSingleton(manager);
            }

            return manager;
        }

        private static void ConfigureApplicationFeatureProviders(ApplicationPartManager manager)
        {
            if (!manager.FeatureProviders.OfType<ControllerFeatureProvider>().Any())
            {
                 manager.FeatureProviders.Add(new ControllerFeatureProvider());
            }
        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        #endregion


        //-- Mvc_FeatureConvetion
        public static void AddTransientMvcFeatureConvetion<TConvetion>(this IServiceCollection services)
           where TConvetion : class, IMvcFeatureConvetion
        {
            services.AddTransient<IMvcFeatureConvetion, TConvetion>();
        }
        public static void AddSingletonMvcFeatureConvetion<TConvetion>(this IServiceCollection services)
           where TConvetion : class, IMvcFeatureConvetion
        {
            services.AddSingleton<IMvcFeatureConvetion, TConvetion>();
        }

        //-- Mvc_ModelProvider
        public static void AddSingletonMvcModelProvider<TModel, TProvider>(this IServiceCollection services)
           where TModel : class
           where TProvider : class, IModelProvider<TModel>
        {
            services.AddSingleton<IModelProvider<TModel>, TProvider>();
        }
        public static void AddSingletonMvcModelProvider<TModel>(this IServiceCollection services, Type provider)
            where TModel : class
        {
            services.AddSingleton(typeof(IModelProvider<TModel>), provider);
        }
        public static void AddSingletonMvcModelProvider<TModel>(this IServiceCollection services, IModelProvider<TModel> provider)
           where TModel : class
        {
            services.AddSingleton(provider);
        }

        //-- Assemply
        public static void AddMvcAssemply(this IServiceCollection services, Assembly assembly)
        {
            AssemblyPart assemblyPart = new AssemblyPart(assembly);

            GetApplicationPartManager(services)
                .ApplicationParts
                .Add(assemblyPart);
        }
        
    }
}
