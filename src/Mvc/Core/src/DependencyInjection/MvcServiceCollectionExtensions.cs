using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Internal;
using EquipApps.Mvc.ModelBinding.Metadata;
using EquipApps.Mvc.ModelBinding.Property;
using EquipApps.Testing.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.Internal;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.Metadata;
using NLib.AtpNetCore.Mvc.ModelBinding.Properties;
using NLib.AtpNetCore.Testing.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Controllers;
using NLib.AtpNetCore.Testing.Mvc.Infrastructure;
using NLib.AtpNetCore.Testing.Mvc.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace NLib.AtpNetCore.Testing
{
    public static class MvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddMvc(this IServiceCollection services)
        {
            var manager = GetApplicationPartManager(services);
            ConfigureApplicationFeatureProviders(manager);
            var builder = new MvcBuilder(services, manager);

            ConfigureDefaultServices(services);

            return builder;
        }

        //-- ModelProvider
        public static void AddMvcModelProvider<TModel, TProvider>(this IServiceCollection services)
           where TModel : class
           where TProvider : class, IModelProvider<TModel>
        {
            services.AddSingleton<IModelProvider<TModel>, TProvider>();
        }
        public static void AddMvcModelProvider<TModel>(this IServiceCollection services, Type provider)
            where TModel : class
        {
            services.AddSingleton(typeof(IModelProvider<TModel>), provider);
        }
        public static void AddMvcModelProvider<TModel>(this IServiceCollection services, IModelProvider<TModel> provider)
           where TModel : class
        {
            services.AddSingleton(provider);
        }

        //-- ModelProvider
        public static void AddMvcAssemply(this IServiceCollection services, Assembly assembly)
        {
            AssemblyPart assemblyPart = new AssemblyPart(assembly);

            GetApplicationPartManager(services)
                .ApplicationParts
                .Add(assemblyPart);
        }

        //-- AddActionDescriptorProvider
        public static void AddActionDescriptorProvider<TProvider>(this IServiceCollection serviceDescriptors)
            where TProvider : class, IActionProvider
        {
            //
            // Action Descriptor Provider
            // Transient
            // 
            serviceDescriptors.TryAddEnumerable(
                ServiceDescriptor.Transient<IActionProvider, TProvider>());
        }










        #region AddMvc Assemply

        public static IMvcBuilder AddAssemply(this IMvcBuilder builder, Assembly assembly)
        {
            AssemblyPart assemblyPart = new AssemblyPart(assembly);

            builder
                .AppPartManager
                .ApplicationParts
                .Add(assemblyPart);

            return builder;
        }
        public static IMvcBuilder AddAssemply<T>(this IMvcBuilder builder)
            where T : class
        {
            return builder.AddAssemply(typeof(T).Assembly);
        }

        #endregion

        #region AddMvc Configure

        public static IMvcBuilder ConfigureApplicationPartManager(this IMvcBuilder builder, Action<ApplicationPartManager> configureApplicationPartManager)
        {
            if (configureApplicationPartManager == null)
            {
                throw new ArgumentNullException(nameof(configureApplicationPartManager));
            }

            configureApplicationPartManager(builder.AppPartManager);

            return builder;
        }

        #endregion


        /// <summary>
        /// Action Descriptor Provider.
        /// Transient
        /// </summary>       
        public static IMvcBuilder AddActionDescriptorProvider<TProvider>(this IMvcBuilder builder)
            where TProvider : class, IActionProvider
        {
            builder.Services.AddActionDescriptorProvider<TProvider>();
           

            return builder;
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

        #endregion

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            // ----------------------------------------------------------------------------------------
            // CORE()
            //
            // 
            services.AddSingleton<MvcMiddleware>();



            // ----------------------------------------------------------------------------------------
            // Action Descriptor Factory
            // Singleton
            // 
            services.AddSingleton<IActionFactory, MvcActionDescriptorFactory>();

            //
            // Action Descriptor Provider
            // Transient
            // 
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IActionProvider, MvcActionDescriptorProvider>());



            // ----------------------------------------------------------------------------------------
            // Application Model Service
            // Singleton
            // 
            services.AddSingleton<IApplicationModelService, MvcApplicationModelService>();

            //
            // Action Descriptor Provider
            //
            services.TryAddEnumerable(
              ServiceDescriptor.Transient<IApplicationModelProvider, MvcApplicationModelProvider>());


            
            // ----------------------------------------------------------------------------------------
















            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IFeatureProvider, MvcFeatureProvider>());

            services.TryAddEnumerable(
               ServiceDescriptor.Transient<IConfigureOptions<MvcOption>, MvcOptionSetup>());

















            //
            // Action Descriptor Provider
            //
            // 

            services.AddSingleton<IActionInvokerProvider, ControllerActionInvokerProvider>();

            //
            // Infrastructure
            //
            // 



            services.AddSingleton<IControllerFactory, ControllerFactory>();


            services.AddSingleton<IPropertyProvider, PropertyProvider>();
            services.AddSingleton<IMetadataProvider, MetadataProvider>();
            services.AddSingleton<IBindingFactory, BindingFactory>();

        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }
    }
}
