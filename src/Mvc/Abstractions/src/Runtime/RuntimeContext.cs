using EquipApps.Testing;
using System;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RuntimeContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>      
        public RuntimeContext(TestContext testContext)
        {
            TestContext = testContext ?? throw new ArgumentNullException(nameof(testContext));          
        }

        /// <summary>
        /// Возвращает <see cref="Testing.TestContext"/>
        /// </summary>
        public TestContext TestContext { get; }

        /// <summary>
        /// Переход на <see cref="ActionDescriptor"/>.
        /// </summary>
        /// 
        /// <param name="actionObject">
        /// Действие на которое произойдет переход
        /// </param>
        /// 
        /// <returns>
        /// true    - переход возможен;
        /// false   - переход не возможен;
        /// </returns>        
        public abstract bool JumpTo(ActionObject actionObject);

        /// <summary>
        /// Переход на <see cref="RuntimeState"/>.
        /// </summary>
        /// 
        /// <param name="stateType">
        /// Состояние вкоторое произойдет переход
        /// </param>
        /// 
        /// <returns>
        /// true    - переход возможен;
        /// false   - переход не возможен;
        /// </returns>       
        public abstract bool JumpTo(RuntimeState runtimeState);
    }
}
