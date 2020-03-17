using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Инфроструктура адаптера устройства
    /// </summary>
    public interface IHardwareAdapter : IDisposable
    {
        /// <summary>
        /// Инициализация адаптера.
        /// Тут происходит логика инициализации устройств.
        /// </summary>
        /// 
        /// <param name="device">
        /// Экземпляр устройства
        /// </param>
        /// 
        /// <param name="deviceName">
        /// Имя устройства
        /// </param>
        void Initialize(IHardwareFeature hardwareManager, object device, string deviceName);

        /// <summary>
        /// Сброс устройства в исходное состояние
        /// </summary>
        void Reset();
    }
}
