using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure
{
    /// <summary>
    /// Фича.
    /// Управляет временем жизни <see cref="ActionObject"/> в <see cref="IActionService"/>.
    /// </summary>
    ///
    public class ViewFeature : IDisposable
    {
        private IActionService actionService;

        /// <summary>
        /// Конструктор
        /// </summary>        
        public ViewFeature(IActionService actionService, IMvcFeature mvcFechure)
        {
            this.actionService = actionService;
            this.actionService.Update(mvcFechure.ActionObjects);
        }

        /// <summary>
        /// Очищает <see cref="ActionService"/>.
        /// Вызывается при уничтожении <see cref="ITest"/> через Dispose
        /// </summary>
        public void Dispose()
        {
            actionService?.Clear();
            actionService = null;
        }
    }
}
