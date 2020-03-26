using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Контекс дейтвия
    /// </summary>
    public class ActionContext
    {
        public ActionContext(ActionContext actionContext)
            : this(actionContext.RuntimeContext,
                  actionContext.ActionDescriptor)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }
        }

        public ActionContext(RuntimeContext runtimeContext,
                             ActionDescriptor actionDescriptor)
        {
            RuntimeContext   = runtimeContext   ?? throw new ArgumentNullException(nameof(runtimeContext));
            ActionDescriptor = actionDescriptor ?? throw new ArgumentNullException(nameof(actionDescriptor));
        }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptor"/>
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; }

        /// <summary>
        /// Возвращает <see cref="Runtime.RuntimeContext"/>
        /// </summary>
        public RuntimeContext RuntimeContext { get; }

        /// <summary>
        /// Возвращает <see cref="TestContext"/>
        /// </summary>
        public TestContext TestContext => RuntimeContext.TestContext;
    }
}
