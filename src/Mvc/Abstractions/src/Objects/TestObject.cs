using System;

namespace EquipApps.Mvc.Objects
{
    /// <summary>
    /// Тестовый обект
    /// </summary>
    public abstract class TestObject : TreeObject
    {
        private volatile Lazy<string> _lzTitle;
        private volatile Lazy<TestNumber> _lzNumber;

        public TestObject()
        {
            _lzTitle  = new Lazy<string>    (GetTitle);
            _lzNumber = new Lazy<TestNumber>(GetNumber);
        }

        /// <summary>
        /// Возвращает номер
        /// </summary>
        public virtual TestNumber Number => _lzNumber.Value;

        /// <summary>
        /// Возвращает Заголовок
        /// </summary>
        public virtual string Title => _lzTitle.Value;

        /// <summary>
        /// Обновляет свое состояние
        /// </summary>
        protected override void UpdatedSelf()
        {
            UpdatedTitle();
            UpdatedNumber();
        }

        /// <summary>
        /// Обновляет свое <see cref="Title"/>
        /// </summary>
        protected virtual void UpdatedTitle()
        {
            if (_lzTitle.IsValueCreated)
            {
                _lzTitle = new Lazy<string>(GetTitle);
            }
        }

        /// <summary>
        /// Обновляет свое <see cref="Number"/>
        /// </summary>
        protected virtual void UpdatedNumber()
        {
            if (_lzNumber.IsValueCreated)
            {
                _lzNumber = new Lazy<TestNumber>(GetNumber);
            }
        }

        /// <summary>
        /// Фабрика <see cref="Title"/>
        /// </summary>      
        protected abstract string GetTitle();

        /// <summary>
        /// Фабрика <see cref="Number"/>
        /// </summary> 
        protected abstract TestNumber GetNumber();
    }
}
