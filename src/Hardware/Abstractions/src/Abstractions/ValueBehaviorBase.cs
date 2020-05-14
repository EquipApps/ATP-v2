using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Реализация базоврого поведения.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValueBehaviorBase<TValue> : IValueBehavior<TValue>, IHardwareBehavior
    {
        #region IHardwareBehavior

        public abstract IHardware Hardware
        {
            get;
            set;
        }

        public abstract void Attach();

        #endregion

        /// <summary>
        /// Возвращает значение
        /// </summary>
        public abstract TValue Value
        {
            get;
            protected set;
        }

        /// <summary>
        /// Request To Update Value.
        /// </summary>
        public virtual void RequestToUpdateValue()
        {
            var valueUpdate = ValueUpdate;
            if (valueUpdate == null)
                throw new InvalidOperationException("Запрос на обновление значения не поддерживается");
            else
                valueUpdate.Invoke(this);
        }

        /// <summary>
        /// Request To Change Value
        /// </summary>        
        public virtual void RequestToChangeValue(TValue value)
        {
            var valueChange = ValueChange;
            if (valueChange == null)
                throw new InvalidOperationException("Запрос на изменение значения не поддерживается");
            else
                valueChange.Invoke(this, value);
        }

        /// <summary>
        /// Set Value
        /// </summary>
        public virtual void SetValue(TValue value)
        {
            Value = value;
        }

        /// <summary>
        /// Событие на обновление данных. (Для адаптера)
        /// </summary>
        public event Action<IValueBehavior<TValue>> ValueUpdate;

        /// <summary>
        /// Событие на изменение данных. (Для адаптера)
        /// </summary>
        public event Action<IValueBehavior<TValue>, TValue> ValueChange;

        /// <summary>
        /// Флаг.
        /// </summary>
        public bool CanUpdateValue => ValueUpdate != null;

        /// <summary>
        /// Флаг.
        /// </summary>
        public bool CanChangeValue => ValueChange != null;
    }
}
