using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Singleton
    /// </summary>
    public class HardwareMiddleware
    {
        private ILogger<HardwareMiddleware> logger;

        public HardwareMiddleware(
            ILogger<HardwareMiddleware> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        internal TestDelegate Add(TestDelegate nextMiddleware)
        {
            NextMiddleware = nextMiddleware ?? throw new ArgumentNullException(nameof(nextMiddleware));

            return RunAsync;
        }

        internal TestDelegate NextMiddleware { get; set; }


        public async Task RunAsync(TestContext testContext)
        {
            if (testContext == null)
            {
                new ArgumentNullException(nameof(testContext));
            }

            //-- Сохраняем следующий Middleware
            var nextMiddleware = NextMiddleware ?? throw new NullReferenceException(nameof(NextMiddleware));

            //-- 

            try
            {
                //-- Сброс устройств в начале
                InitiallyHardwareReset(testContext);

                //-- Переходим дальше по конвееру
                await nextMiddleware(testContext);
            }
            finally
            {
                //-- Сброс устройств в конце
                FinallyHardwareReset(testContext);
            }
        }

        private void InitiallyHardwareReset(TestContext testContext)
        {
            //TODO: Тут можно добавить условие пропуска сброса

            HardwareReset(testContext);
        }

        private void FinallyHardwareReset(TestContext testContext)
        {
            //TODO: Тут можно добавить условие пропуска сброса

            HardwareReset(testContext);
        }

        private void HardwareReset(TestContext testContext)
        {
            var hardwareFeature = testContext.GetHardwareFeature();

            foreach (var hardwareAdapter in hardwareFeature.HardwareAdapters)
            {
                hardwareAdapter.Reset();
            }
        }
    }
}
