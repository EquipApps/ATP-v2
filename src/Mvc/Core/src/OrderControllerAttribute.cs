using EquipApps.Mvc.Routing;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// <para>
    /// Определяет порядковый номер для Контроллера.
    /// </para>
    ///
    /// Можно примять к контроллерам и методам.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class OrderControllerAttribute : OrderValueAttribute
    {
        public OrderControllerAttribute(string order)
            : base("controller", order)
        {

        }
    }
}
