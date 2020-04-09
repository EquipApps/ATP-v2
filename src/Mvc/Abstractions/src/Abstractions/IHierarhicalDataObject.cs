namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Древовидный объект. (Поддержка DataContext)
    /// </summary>
    public interface IHierarhicalDataObject : IDataObject
    {
        /// <summary>
        /// Возвращает родительский элемент
        /// </summary>
        IHierarhicalDataObject Parent { get; }

        /// <summary>
        /// Возвращает число дочерних элементов
        /// </summary>
        int ChildrenCount { get; }

        /// <summary>
        /// Возвращает <see cref="IHierarhicalDataObject"/>
        /// </summary>      
        IHierarhicalDataObject GetChild(int index);

    }
}
