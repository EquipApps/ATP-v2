using EquipApps.Testing.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace EquipApps.Testing
{
    /// <summary>
    /// Содержит всю информацию о тестовой проверке.
    /// </summary>
    public abstract class TestContext
    {
        /// <summary>
        /// Возвращает <see cref="IFeatureCollection"/>
        /// </summary>
        public abstract IFeatureCollection TestFeatures { get; }

        /// <summary>
        /// Возвращает <see cref="ILogger"/>
        /// </summary>
        public abstract ILogger TestLogger { get; }

        /// <summary>
        /// Возвращает <see cref="Testing.TestOptions"/>
        /// </summary>
        public abstract TestOptions TestOptions { get; }

        /// <summary>
        /// Возвращает <see cref="IServiceProvider"/>
        /// </summary>
        public abstract IServiceProvider TestServices { get; }

        /// <summary>
        /// Возвращает <see cref="CancellationToken"/> 
        /// </summary>
        public abstract CancellationToken TestAborted { get; }
    }
}
