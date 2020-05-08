using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Кешированная коллекция <see cref="ActionDescriptor" /> элементов.
    /// </summary>
    public class ActionDescriptorCollection
    {
        /// <summary>
        /// Создает новый экземпляр <see cref="ActionDescriptorCollection"/>.
        /// </summary>        
        public ActionDescriptorCollection(IReadOnlyList<ActionDescriptor> items, int version)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Items = items;
            Version = version;
        }

        /// <summary>
        /// Возвращает кэш <see cref="IReadOnlyList{ActionDescriptor}"/>.
        /// </summary>
        public IReadOnlyList<ActionDescriptor> Items { get; }

        /// <summary>
        /// Возвращает версию кэша.
        /// </summary>
        public int Version { get; }
    }
}
