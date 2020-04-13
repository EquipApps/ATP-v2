using EquipApps.Testing.Wpf;
using EquipApps.WorkBench.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;
using System;

namespace EquipApps.WorkBench
{
    public abstract class WbApp : TestApplication
    {
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

                resolver.RegisterViewsForViewModels(typeof(WbApp).Assembly);

            //-- Вызываем баззовую функцию конфигурации
            base.ConfigureServiceCollectionDefault(serviceCollection);

            //TODO: Перенеси как расширение в Core
            //-------------------------------------------------------------
            serviceCollection.AddViewer();


            //-------------------------------------------------------------
            serviceCollection.AddTransient<LogViewModel>();
            serviceCollection.AddTransient<TestViewerViewModel>();
            serviceCollection.AddTransient<WorkViewerViewModel>();



            serviceCollection.AddTransient<ActionsByResultTool>();



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
