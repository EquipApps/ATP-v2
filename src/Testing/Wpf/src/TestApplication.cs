using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace EquipApps.Testing.Wpf
{
    public abstract class TestApplication : Application, IStartup
    {
        /// <summary>
        /// Возвращает <see cref="IServiceProvider"/>.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeCore();
        }

        /// <summary>
        /// Инициализация инфроструктуры
        /// </summary>
        protected virtual void InitializeCore()
        {
            var serviceCollection = GetServiceCollection();

            ConfigureServiceCollectionDefault   (serviceCollection);
            ConfigureServiceCollection          (serviceCollection);

            ServiceProvider = GetServiceProvider(serviceCollection);
        }

        /// <summary>
        /// Создает <see cref="IServiceCollection"/>
        /// </summary>      
        protected virtual IServiceCollection GetServiceCollection()
        {
            return new ServiceCollection();
        }

        /// <summary>
        /// Конфигурация <see cref="IServiceCollection"/> по умолчанию.
        /// </summary>        
        protected virtual void ConfigureServiceCollectionDefault(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging(Configure);

            //---
            serviceCollection.AddTransient<IStartup>((_) => this);
            serviceCollection.AddTransient<ITestFactory, DafaultTestFactory>();
        }

        /// <summary>
        /// Конфигурация <see cref="IServiceCollection"/>.
        /// </summary>  
        protected abstract void ConfigureServiceCollection(IServiceCollection serviceCollection);
        
        /// <summary>
        /// Создает <see cref="IServiceProvider"/>
        /// </summary>    
        protected virtual IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }
        
        /// <summary>
        /// Конфигурация логера
        /// </summary>        
        protected abstract void Configure(ILoggingBuilder builder);

        /// <summary>
        /// Конфигурация тестовой проверки
        /// </summary>
        protected abstract void Configure(ITestBuilder builder);




        IServiceProvider IStartup.GetServiceProvider()
        {
            return ServiceProvider;
        }

        void IStartup.Configure(ITestBuilder builder)
        {
            this.Configure(builder);
        }
    }
}
