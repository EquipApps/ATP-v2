using EquipApps.Testing;
using EquipApps.Testing.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Internal
{
    public partial class DafaultTest : ITest, IDisposable
    {
        private IFeatureCollection features;
        private ILogger logger;
        private TestOptions options;
        private IServiceProvider serviceProvider;
        private TestDelegate app;
        private volatile bool isProcess = false;


        public DafaultTest(
            IFeatureCollection features,
            ILogger logger,
            TestOptions options,
            IServiceProvider serviceProvider,
            TestDelegate app)
        {
            this.features = features;
            this.logger = logger;
            this.options = options;
            this.serviceProvider = serviceProvider;
            this.app = app;
        }


        public async Task ProcessAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                isProcess = true;

                //-- Создаем тестовый контекст
                var testContext = new DefaultTestContext(
                    features, logger, options, serviceProvider, cancellationToken);

                await app(testContext);


            }
            finally
            {
                isProcess = false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    (features as IDisposable)?.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                features = null;
                logger = null;
                options = null;
                serviceProvider = null;
                app = null;

                disposedValue = true;
            }
        }


        ~DafaultTest()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
