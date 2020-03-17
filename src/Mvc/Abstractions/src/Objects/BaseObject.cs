namespace EquipApps.Mvc.Objects
{
    /// <summary>
    /// Базовый объект. (Поддержка DataContext)
    /// </summary>
    public abstract class BaseObject
    {
        /// <summary>
        /// Задает или возвращает контекст данных.
        /// </summary>        
        public virtual object DataContext
        {
            get => _dataContext;
            set
            {
                var oldDataContext = _dataContext;
                if (oldDataContext != value)
                {
                    _dataContext = value;
                    Update();
                }
            }
        }
        private volatile object _dataContext;

        /// <summary>
        /// Обновляет свое состояние
        /// </summary>
        public abstract void Update();
    }
}
