using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Фабрика <see cref="IActionInvoker"/>
    /// </summary>
    public interface IActionInvokerFactory : IDisposable
    {
        /// <summary>
        /// Создает <see cref="IActionInvoker"/> для <see cref="ActionContext"/>
        /// </summary>      
        IActionInvoker CreateInvoker(ActionContext actionContext);
    }
}
