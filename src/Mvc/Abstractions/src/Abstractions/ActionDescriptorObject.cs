namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Тестовый обект
    /// </summary>
    public abstract class ActionDescriptorObject : IHierarhicalDataObject
    {
        public ActionDescriptorObject()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        /// Возвращает Заголовок
        /// </summary>
        public virtual string Title { get; set; }

        public virtual Number Number { get; set; }















        public abstract IHierarhicalDataObject Parent { get; set; }

        public abstract int ChildrenCount { get; }

        public abstract IHierarhicalDataObject GetChild(int index);
    }
}
