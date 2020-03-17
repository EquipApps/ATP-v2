using EquipApps.Testing.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace EquipApps.Testing.Wpf.Internal
{
    /// <summary>
    /// Реализация <see cref="TestContext"/> по умолчанию
    /// </summary>
    public class DefaultTestContext : TestContext
    {
        public DefaultTestContext(
            IFeatureCollection features,
            ILogger logger,
            TestOptions options,
            IServiceProvider serviceProvider,
            CancellationToken cancellation)
        {
            this.TestFeatures = features;
            this.TestLogger = logger;
            this.TestOptions = options;
            this.TestServices = serviceProvider;
            this.TestAborted = cancellation;
        }

        public override IFeatureCollection TestFeatures { get; }

        public override ILogger TestLogger { get; }

        public override TestOptions TestOptions { get; }

        public override IServiceProvider TestServices { get; }

        public override CancellationToken TestAborted { get; }
    }
}
