﻿using System.Collections.Generic;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Интератор с возможностью перехода
    /// </summary>
    public interface IActionDescriptorEnumerator : IEnumerator<ActionDescriptor>
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