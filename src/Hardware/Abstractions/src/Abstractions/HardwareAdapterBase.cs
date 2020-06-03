using System;

namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Базовая реализация инфроструктуры <see cref="IHardwareAdapter"/>.
    /// Используется для взаимодействия с <typeparamref name="TDevice"/> устройством.
    /// </summary>
    /// 
    /// <typeparam name="TDevice">
    /// Тип Устройства
    /// </typeparam>
    public abstract class HardwareAdapterBase<TDevice> : IHardwareAdapter
        where TDevice : class
    {
        /// <summary>
        /// Возвращает <see cref="IHardwareFeature"/>.
        /// Значения присваивается во время инициализации
        /// </summary> 
        public IHardwareFeature HardwareFeature { get; private set; }

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// 
        /// <param name="hardwareFeature">
        /// Менеджер устройств
        /// </param>
        /// 
        /// <param name="device">
        /// Устройство
        /// </param>
        /// 
        /// <param name="deviceName">
        /// Имя устройства
        /// </param>        
        /// 
        /// <remarks>
        /// Вызывает последовательно функции:
        /// 1) <see cref="Adapt(TDevice, string)"/>
        /// 2) <see cref="AttachBehaviors"/>
        /// </remarks>
        public void Initialize(IHardwareFeature hardwareFeature, object device, string deviceName)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (deviceName == null)
            {
                throw new ArgumentNullException(nameof(deviceName));
            }

            //-- Сохраняем
            HardwareFeature = hardwareFeature ?? throw new ArgumentNullException(nameof(hardwareFeature));

            Adapt(device as TDevice, deviceName);

            AttachBehaviors();

            InitializeDevice();
        }


        /// <summary>
        /// Инициализация инфроструктуры адаптера.
        /// Вызывается во время инициализации.
        /// </summary>
        /// 
        /// <param name="device">
        /// Устройство
        /// </param>
        /// 
        /// <param name="deviceName">
        /// Имя устройства
        /// </param>
        protected abstract void Adapt(TDevice device, string deviceName);

        /// <summary>
        /// Привязка поведений.
        /// Вызывается во время инициализации.
        /// <para>(По умолчаню пуст)</para>
        /// </summary>
        protected virtual void AttachBehaviors()
        {

        }

        /// <summary>
        /// Инициализация устройства. (По умолчаню пуст)
        /// Вызывается во время инициализации.
        /// </summary>
        protected virtual void InitializeDevice()
        {

        }

        /// <summary>
        /// Сброс устройств в исходное состояние. (По умолчаню пуст)
        /// </summary>
        protected virtual void ResetDevice()
        {

        }


        void IHardwareAdapter.Reset()
        {
            ResetDevice();
        }

        public abstract void Dispose();

    }
}
