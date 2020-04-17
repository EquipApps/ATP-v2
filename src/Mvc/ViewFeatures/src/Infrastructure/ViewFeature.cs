using EquipApps.Mvc.Services;
using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Фича.
    /// Управляет временем жизни <see cref="ActionDescriptor"/> в <see cref="ActionService"/>.
    /// </summary>
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
