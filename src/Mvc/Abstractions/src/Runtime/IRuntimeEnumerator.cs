using EquipApps.Mvc.Abstractions;
using System.Collections.Generic;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRuntimeEnumerator : IEnumerator<ActionDescriptor>
    {
        /// <summary>
        /// Переход на <see cref="ActionDescriptor"/>.
        /// </summary>
        /// 
        /// <param name="actionDescriptor">
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
        bool JumpTo(ActionDescriptor actionDescriptor);
    }
}