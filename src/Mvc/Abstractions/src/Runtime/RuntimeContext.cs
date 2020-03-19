using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Runtime
{
    public class RuntimeContext
    {
        public RuntimeContext(
            TestContext testContext,
            IRuntimeActionEnumerator       actionEnumerator,
            IActionInvokerFactory   actionInvokerFactory,
            IRuntimeStateEnumerator        stateEnumerator)
        {
            TestContext = testContext      ?? throw new ArgumentNullException(nameof(testContext));
            Action  = actionEnumerator     ?? throw new ArgumentNullException(nameof(actionInvokerFactory));
            Factory = actionInvokerFactory ?? throw new ArgumentNullException(nameof(actionEnumerator));
            State   = stateEnumerator      ?? throw new ArgumentNullException(nameof(stateEnumerator));
        }

        /// <summary>
        /// Флаг завершения обработки
        /// </summary>
        public bool Handled { get; set; } = false;

        /// <summary>
        /// Возвращает <see cref="Testing.TestContext"/>
        /// </summary>
        public TestContext TestContext { get; }

        /// <summary>
        /// Возвращает <see cref="IRuntimeActionEnumerator"/>
        /// </summary>
        public IRuntimeActionEnumerator Action { get; }

        /// <summary>
        /// Возвращает <see cref="IActionInvokerFactory"/>
        /// </summary>
        public IActionInvokerFactory Factory { get; }

        /// <summary>
        /// Возвращает <see cref="IRuntimeStateEnumerator"/>
        /// </summary>
        public IRuntimeStateEnumerator State { get; }
    }
}
