namespace EquipApps.Mvc.ApplicationParts
{
    /// <summary>
    /// Часть приложения
    /// </summary>
    public abstract class ApplicationPart
    {
        /// <summary>
        /// Возвращает Имя
        /// </summary>
        public abstract string Name { get; }
    }
}
