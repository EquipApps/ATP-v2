namespace EquipApps.Hardware
{
    /// <summary>
    /// Инфроструктура "виртуального" устройства
    /// </summary>
    public interface IHardware
    {
        /// <summary>
        /// Возвращает ключ устройства. Уникальное!
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Задает или Возвращает имя устройства.
        /// По умолчани совпадает с ключом
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Задает или Возвращает описание устройства.
        /// По умолчанию пустая строка.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Возвращает <see cref="IHardwareBehaviorCollection"/> для данного устройства
        /// </summary>
        IHardwareBehaviorCollection Behaviors { get; }
    }
}
