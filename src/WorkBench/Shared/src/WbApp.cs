using EquipApps.Mvc.Reactive.ViewFeatures.Viewers;
using EquipApps.Testing.Wpf;
using EquipApps.WorkBench.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Testing;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;
using System;

namespace EquipApps.WorkBench
{
    public abstract partial class WbApp : TestApplication
    {
        public WbApp()
        {
            InitializeResources();
        }
        
        /// <summary>
        /// Инициализация ресурсов.
        /// </summary>
        partial void InitializeResources();

        /// <summary>
        /// Инициализация ресурсов.
        /// </summary>
        partial void ConfigureServiceForShell(IServiceCollection serviceCollection);

        /// <summary>
        /// Переопределяем сервисы по умолчанию.
        /// Splat интеграция.
        /// </summary>        
        protected override void ConfigureServiceCollectionDefault(IServiceCollection serviceCollection)
        {
            //-- Переопределяем Splat.Locator
            serviceCollection.UseMicrosoftDependencyResolver();

            //-- Регистрация Инфроструктуры сплат!
            var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();             //-- Splat
                resolver.InitializeReactiveUI();        //-- ReactiveUI
                
            //-- Теперь Splat пише в Extensions.Logging
            resolver.RegisterConstant(new FuncLogManager(FuncLogFactory), typeof(ILogManager));

            //-- Регистрируем все IViewFor
            resolver.RegisterViewsForViewModels(typeof(WbApp).Assembly);


            //TODO: Перенеси в ConfigureServiceCollection
            //-------------------------------------------------------------   
            //-- Mvc
            serviceCollection.AddMvc();
            serviceCollection.AddMvcLogs();
            serviceCollection.AddMvcView();
            serviceCollection.AddMvcWork();

            //-- Hardware
            serviceCollection.AddHardware();           
            serviceCollection.AddHardwareDigital();
            //-------------------------------------------------------------


            serviceCollection.AddTransient<WorkViewerViewModel>();

            //-------------------------------------------------------------
            //-- Регистрация оболочки
            ConfigureServiceForShell(serviceCollection);

            //-------------------------------------------------------------
            //-- ВАЖНО: Вызывать в конце.
            //--        1) Позволяет очистить ILoggerProvider
            //-- Вызываем баззовую функцию конфигурации
            base.ConfigureServiceCollectionDefault(serviceCollection);

        }

        /// <summary>
        /// Переопределяем логику создания <see cref="IServiceProvider"/>.
        /// Splat интеграция.
        /// </summary>      
        protected override IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            var serviceProvider = base.GetServiceProvider(serviceCollection);
                serviceProvider.UseMicrosoftDependencyResolver();

            return serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>     
        private IFullLogger FuncLogFactory(Type type)
        {
            var logFactory    = ServiceProvider.GetService<ILoggerFactory>();
            var logger        = logFactory.CreateLogger(type.ToString());
            var loggerWrapper = new MicrosoftExtensionsLoggingLogger(logger);
            return new WrappingFullLogger(loggerWrapper);
        }
    }
}
