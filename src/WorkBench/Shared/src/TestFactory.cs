using EquipApps.Testing;
using EquipApps.Testing.Features;
using EquipApps.WorkBench.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EquipApps.WorkBench
{
    public class TestFactory : ITestFactory
    {
        private IStartup _startup;
        private TestOptions _options;

        public TestFactory(IStartup startup, IOptions<TestOptions> options)
        {
            _startup = startup        ?? throw new ArgumentNullException(nameof(startup));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public ITest CreateTest()
        {
            //-- Создаем провайдер сервисов
            var serviceProvider = _startup.GetServiceProvider();

            //-- Извлекаем провайдер
            var featureProviders = serviceProvider
                .GetServices<IFeatureProvider>()
                .ToArray();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger        = loggerFactory.CreateLogger<TestContext>();

            //-- Создаем расширения
            var featureCollection = new DefaultTestFeatureCollection();// GetFeatureCollection(featureProviders);

            //-- Создаем Билдер
            var builder = new DefaultTestDelegateBuilder(
                serviceProvider,
                featureCollection,
                _options);

            _startup.Configure(builder);

            var pipeline = builder.Build();

            //-- Создаем расширения
            var featureCollection2 = GetFeatureCollection(featureProviders);

            //-- Возвращаем тестовую проверку
            return new DafaultTest(featureCollection2, logger, _options, serviceProvider, pipeline);
        }

        private IFeatureCollection GetFeatureCollection(IFeatureProvider[] featureProviders)
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

        public Task<ITest> CreateTestAsync()
        {
            return Task.Run(CreateTest);
        }
    }
}
