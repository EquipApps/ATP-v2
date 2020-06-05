namespace EquipApps.Hardware.Behaviors.Digital
{
    /// <summary>
    /// Инфроструктура цифрового порта
    /// </summary>
    public interface IDigitalPort
    {
        /// <summary>
        /// Флаг. Можно ли обновлять значение
        /// </summary>
        bool CanUpdateValue { get; }

        /// <summary>
        /// Флаг. Можно ли изменять значение
        /// </summary>
        bool CanChangeValue { get; }
    }
}
