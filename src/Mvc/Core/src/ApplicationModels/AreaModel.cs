using System;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель области изоляции.
    /// Введена для разделения проверок в разнве группы.
    /// </summary>
    public class AreaModel
    {
        public AreaModel(int index, string name)
        {
            Index = index;
            Name  = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Индес.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }
    }
}
