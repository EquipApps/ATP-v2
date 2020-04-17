using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using EquipApps.Testing;
using System;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Промежуточный слой.
    /// </summary>
    /// 
    public class ViewMiddleware
    {
        private IActionService _actionService;

        public ViewMiddleware(IActionService actionService)
        {
            _actionService = actionService ?? throw new ArgumentNullException(nameof(actionService));
        }

        public Task RunUpdateAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }

        private void Run(TestContext testContext)
        {
            //-- 1) Извлечение дескриптеров действий
            var actionObjects = testContext.GetActionObjects();

            //-- 2) Обновляем состояние.
            foreach (var actionDescriptor in actionObjects)
            {
                actionDescriptor.SetResult(ActionObjectResultType.NotRun);
            }

            _actionService.Update(actionObjects);
        }
    }
}
