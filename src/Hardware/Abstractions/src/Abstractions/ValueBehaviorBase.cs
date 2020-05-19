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

        public virtual IHardware Hardware
        {
            get;
            set;
        }

        /// <inheritdoc/>  
        public virtual void Attach()
        {

        }

        #endregion

        /// <inheritdoc/>   
        public virtual TValue Value
        {
            get;
            protected set;
        }

        /// <inheritdoc/>        
        public virtual void RequestToUpdateValue()
        {
            var valueUpdate = ValueUpdate;
            if (valueUpdate == null)
                throw new InvalidOperationException("Запрос на обновление значения не поддерживается");
            else
                valueUpdate.Invoke(this);
        }

        /// <inheritdoc/>         
        public virtual void RequestToChangeValue(TValue value)
        {
            var valueChange = ValueChange;
            if (valueChange == null)
                throw new InvalidOperationException("Запрос на изменение значения не поддерживается");
            else
                valueChange.Invoke(this, value);
        }

        /// <inheritdoc/>    
        public virtual void SetValue(TValue value)
        {
            Value = value;
        }

        /// <summary>
        /// Событие на изменение данных. (Для адаптера)
        /// </summary>   
        public event Action<ValueBehaviorBase<TValue>> ValueUpdate;

        /// <summary>
        /// Событие на обновление данных. (Для адаптера)
        /// </summary>
        public event Action<ValueBehaviorBase<TValue>, TValue> ValueChange;

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
