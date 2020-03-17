using System.Collections.Generic;

namespace EquipApps.Mvc.Runtime
{
    public interface IRuntimeStateEnumerator : IEnumerator<IRuntimeState>
    {
        /// <summary>
        /// Переход на <see cref="RuntimeStateType"/>.
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
        /// 
        /// <remarks>
        /// Переход произойдет после вызова функции MoveNext;
        /// </remarks>
        /// 
        bool JumpTo(RuntimeStateType stateType);
    }
}
