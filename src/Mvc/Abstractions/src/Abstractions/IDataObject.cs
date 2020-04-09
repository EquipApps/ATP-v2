namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Базовый объект. (Поддержка DataContext)
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        /// Задает или возвращает контекст данных.
        /// </summary>        
        object DataContext
        {
            get;
        }
    }
}
