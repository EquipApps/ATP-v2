using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using EquipApps.Testing;
using System;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Промежуточный слой.
    /// </summary>
    /// 
    public class LogsMiddleware
    {
        private ILogService _service;

        public LogsMiddleware(ILogService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Запускает задачу очистки журнала сообщений
        /// </summary>
        public Task RunCleanAsync(TestContext testContext)
        {
            return _service.CleanAsync();
        }
    }
}
