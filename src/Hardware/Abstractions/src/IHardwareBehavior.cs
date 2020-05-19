namespace EquipApps.Hardware
{
    // <summary>
    /// Поведение виртуального устройства
    /// </summary>
    public interface IHardwareBehavior
    {
        /// <summary>
        /// Задает или возвращает <see cref="IHardware"/> связанное с данным поведением.
        /// </summary>
        IHardware Hardware { get; set; }

        /// <summary>
        /// Функция привязки поведения к устройству.
        /// <para>Вызывается в момент когда <see cref="Hardware"/> установлен</para>
        /// 
        /// </summary>
        void Attach();
    }
}
