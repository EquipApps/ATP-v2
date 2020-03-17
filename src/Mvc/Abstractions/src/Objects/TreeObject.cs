namespace EquipApps.Mvc.Objects
{
    /// <summary>
    /// Древовидный объект. (Поддержка DataContext)
    /// </summary>
    public abstract class TreeObject : BaseObject
    {
        /// <summary>
        /// Возвращает родительский элемент
        /// </summary>
        public TreeObject Parent { get; protected set; }

        /// <summary>
        /// Обновляет свое состояние и состояние всех дочерних элементов.
        /// </summary>
        public override void Update()
        {
            UpdatedSelf();
            UpdateChild();
        }

        protected virtual void UpdatedSelf()
        {
        }

        protected virtual void UpdateChild()
        {
            for (int i = 0; i < ChildrenCount; i++)
            {
                var element = GetChild(i);
                element.Update();
            }
        }

        /// <summary>
        /// Возвращает число дочерних элементов
        /// </summary>
        public abstract int ChildrenCount { get; }

        /// <summary>
        /// Возвращает <see cref="TreeObject"/>
        /// </summary>      
        public abstract TreeObject GetChild(int index);

    }
}
