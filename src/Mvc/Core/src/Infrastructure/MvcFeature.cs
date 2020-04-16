using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Infrastructure
{
    public class MvcFeature : IMvcFeature, IDisposable
    {
        /// <inheritdoc/>
        public IReadOnlyList<ActionDescriptor> ActionDescriptors { get; set; }

        public void Dispose()
        {
            /*
             * Обнуляем ссылку.
             * Данный метод вызывается по премя вызова ITest.Dispose
             * 
             * ВАЖНО. Не ВЫЗЫВАТЬ Dispose для каждого ActionDescriptor.
             * Т.к ActionDescriptor кеширован. Могут возникнуть баги.
             */
            ActionDescriptors = null;
        }
    }
}
