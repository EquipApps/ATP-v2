using EquipApps.Testing.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipApps.Testing.Wpf.Internal
{
    public class DefaultTestDelegateBuilder : ITestBuilder
    {
        private readonly IList<Func<TestDelegate, TestDelegate>> _components = new List<Func<TestDelegate, TestDelegate>>();


        public DefaultTestDelegateBuilder(
            IServiceProvider applicationServices,
            IFeatureCollection applicationFeatures,
            TestOptions options)
        {
            this.ApplicationServices = applicationServices ?? throw new ArgumentNullException(nameof(applicationServices));
            this.ApplicationFeatures = applicationFeatures ?? throw new ArgumentNullException(nameof(applicationFeatures));
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public DefaultTestDelegateBuilder(DefaultTestDelegateBuilder testBuilder)
        {
            this.ApplicationServices = testBuilder.ApplicationServices;
            this.ApplicationFeatures = testBuilder.ApplicationFeatures;
        }


        public IServiceProvider ApplicationServices { get; }

        public IFeatureCollection ApplicationFeatures { get; }

        public TestOptions Options { get; }

        public ITestBuilder New()
        {
            return new DefaultTestDelegateBuilder(this);
        }

        public ITestBuilder Use(Func<TestDelegate, TestDelegate> middleware)
        {
            _components.Add(middleware);

            return this;
        }

        public TestDelegate Build()
        {
            TestDelegate app = context =>
            {
                return Task.CompletedTask;
            };

            foreach (var component in _components.Reverse())
            {
                app = component(app);
            }

            return app;
        }
    }
}
