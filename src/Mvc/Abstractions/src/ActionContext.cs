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
                   actionContext.ActionObject)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }
        }

        public ActionContext(RuntimeContext runtimeContext,
                             ActionObject actionObject)
        {
            RuntimeContext  = runtimeContext   ?? throw new ArgumentNullException(nameof(runtimeContext));
            ActionObject    = actionObject     ?? throw new ArgumentNullException(nameof(actionObject));
        }

        /// <summary>
        /// Возвращает <see cref="ActionObject"/>
        /// </summary>
        public ActionObject ActionObject { get; }

        public ActionDescriptor ActionDescriptor => ActionObject.ActionDescriptor;

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
