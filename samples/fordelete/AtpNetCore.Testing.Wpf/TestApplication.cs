using EquipApps.Testing;
using EquipApps.WorkBench.Controls.BatteryViewer;
using EquipApps.WorkBench.Controls.RelayViewer;
using EquipApps.WorkBench.Services;
using EquipApps.WorkBench.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;

using System;
using System.Windows;

namespace EquipApps.WorkBench
{
    public abstract class TestApplication : Application, IStartup
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeCore();
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected virtual void InitializeCore()
        {
            var serviceCollection = new ServiceCollection();

            //-- Переопределяем Splat.Locator
            serviceCollection.UseMicrosoftDependencyResolver();

            //-- Регистрация инфроструктуры!
            var resolver = Locator.CurrentMutable;            
                resolver.InitializeSplat();             //-- Splat
                resolver.InitializeReactiveUI();        //-- ReactiveUI
                                                        //-- Теперь Splat пише в Extensions.Logging
                resolver.RegisterConstant(new FuncLogManager(FuncLogFactory), typeof(ILogManager));
            Configure(resolver);

            //-- Регистрация сервисов
            ConfigureDefault(serviceCollection);
            Configure       (serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
                serviceProvider.UseMicrosoftDependencyResolver();

            ServiceProvider = serviceProvider;
        }

        protected abstract void Configure(IMutableDependencyResolver resolver);

        private IFullLogger FuncLogFactory(Type type)
        {
            var logFactory = ServiceProvider.GetService<ILoggerFactory>();
            var logger = logFactory.CreateLogger(type.ToString());
            var loggerWrapper = new MicrosoftExtensionsLoggingLogger(logger);
            return new WrappingFullLogger(loggerWrapper);
        }

        protected virtual void ConfigureDefault(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging(Configure);
            //-------------------------------------------------------------
            serviceCollection.AddSingleton<IStartup>         (this);
            serviceCollection.AddSingleton<ILogEntryService, LogEntryService>();
            serviceCollection.AddTransient<ITestFactory,     TestFactory>();

            //-------------------------------------------------------------
            serviceCollection.AddTransient<LogViewerViewModel>();
            serviceCollection.AddTransient<TestViewerViewModel>();
            serviceCollection.AddTransient<WorkViewerViewModel>();

            //-------------------------------------------------------------
            serviceCollection.AddTransient<BatteryViewerViewModel>();
            serviceCollection.AddTransient<RelayViewerViewModel>();
        }

        protected virtual void Configure(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        }
        protected abstract void Configure(IServiceCollection serviceCollection);
        public abstract void Configure(ITestBuilder builder);









        IServiceProvider IStartup.GetServiceProvider()
        {
            return ServiceProvider;
        }
    }
}
