using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure
{
    /// <summary>
    /// Интератор с возможностью перехода
    /// </summary>
    public interface IActionObjectEnumerator : IEnumerator<ActionObject>
    {
        /// <summary>
        /// Переход на <see cref="ActionObject"/>.
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
        /// 
        /// <remarks>
        /// Переход произойдет после вызова функции MoveNext;
        /// </remarks>
        /// 
        bool JumpTo(Predicate<ActionObject> predicate);
    }
}