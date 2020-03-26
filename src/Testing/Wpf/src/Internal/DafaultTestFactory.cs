using EquipApps.Testing.Features;
using EquipApps.Testing.Wpf.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipApps.Testing.Wpf
{
    public class DafaultTestFactory : ITestFactory
    {
        private IStartup        _startup;
        private ILoggerFactory  _loggerFactory;
        private TestOptions     _options;
        private IFeatureProvider[] _providers;

        public DafaultTestFactory(
            IStartup startup, 
            IOptions<TestOptions> options,
            IEnumerable<IFeatureProvider> providers,
            ILoggerFactory loggerFactory   )
        {
            _startup        = startup               ?? throw new ArgumentNullException(nameof(startup));
            _options        = options?.Value        ?? throw new ArgumentNullException(nameof(options));
            _loggerFactory  = loggerFactory         ?? throw new ArgumentNullException(nameof(loggerFactory));


            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = providers
                .OrderBy(x => x.Order)
                .ToArray();
        }

        public Task<ITest> CreateTestAsync()
        {
            return Task.Run(CreateTest);
        }

        public ITest CreateTest()
        {
            //-- Создаем логер
            var logger = _loggerFactory.CreateLogger<TestContext>();

            //-- Создаем провайдер сервисов
            var serviceProvider     = _startup.GetServiceProvider();

            //-- Создаем расширения
            var featureCollection   = GetFeatureCollection(_providers);

            //-- Создаем Билдер
            var builder = new DefaultTestDelegateBuilder(
                serviceProvider,
                featureCollection,
                _options);

            _startup.Configure(builder);

            var pipeline = builder.Build();

            //-- Возвращаем тестовую проверку
            return new DafaultTest(featureCollection, logger, _options, serviceProvider, pipeline);
        }

        public static IFeatureCollection GetFeatureCollection(IFeatureProvider[] featureProviders)
        {
            var featureCollection = new DefaultTestFeatureCollection();
            var featureContext    = new FeatureProviderContext(featureCollection);

            for (var i = 0; i < featureProviders.Length; i++)
            {
                featureProviders[i].OnProvidersExecuting(featureContext);
            }
            for (var i = featureProviders.Length - 1; i >= 0; i--)
            {
                featureProviders[i].OnProvidersExecuted(featureContext);
            }

            return featureCollection;
        }
    }
}
