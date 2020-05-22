using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Инфроструктура соотвецтвия устройство адаптер
    /// </summary>
    public struct HardwareAdapterMap
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Тип адаптера 
        /// </summary>
        public Type AdapterType { get; internal set; }

        /// <summary>
        /// Тип устройства
        /// </summary>
        public Type DeviceType { get; internal set; }

        /// <summary>
        /// Фабричный метод. Создает устройство
        /// </summary>
        public Func<object> Factory { get; internal set; }
    }
}
