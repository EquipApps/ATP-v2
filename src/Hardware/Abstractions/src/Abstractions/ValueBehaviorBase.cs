using System;

namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Реализация поведения значения.
    /// </summary>    
    /// 
    /// <remarks>
    /// ОПИСАНИЕ:
    ///  
    ///  |A|                    |B|  <---- (Request)    |E| 1) Расширение отправляет запросы через поведение
    ///  |D| <---- (Event)      |E|                     |X| 2) Поведение отовещает адаптер через событие
    ///  |A|  ---> (Context)    |H|                     |T| 3) Адаптер обрабатывает событие 
    ///  |P|                    |A|  ----> (Value)      |E| и устанавливает результат обработки через контекст события
    ///  |T|                    |V|                     |N| 4) Поведение анализирует состояние контекста и изменяет состояние буферного значения Value
    ///  |E|                    |I|                     |T| 5) Расширение анализирует значение Value
    ///  |R|                    |O|                     |I|
    ///                         |R|                     |O|
    ///                                                 |N|
    /// 
    /// </remarks>
    /// 
    public abstract class ValueBehaviorBase<TValue> : IValueBehavior<TValue>, IHardwareBehavior
    {
        #region IHardwareBehavior

        /// <inheritdoc/>  
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

            var context = new ValueBehaviorContext<TValue>(Value);

            valueUpdate.Invoke(context);

            var output = context.GetOutput();

            SetValue(output);
        }

        /// <inheritdoc/>         
        public virtual void RequestToChangeValue(TValue value)
        {
            var valueChange = ValueChange;
            if (valueChange == null)
                throw new InvalidOperationException("Запрос на изменение значения не поддерживается");

            var context = new ValueBehaviorContext<TValue>(value);
            
            valueChange.Invoke(context);

            var output = context.GetOutput();

            SetValue(output);
        }

        /// <summary>
        /// Изменяет <see cref="Value"/>
        /// </summary>
        public virtual void SetValue(TValue value)
        {
            Value = value;
        }

        /// <summary>
        /// Событие на изменение данных. (Для адаптера)
        /// </summary>   
        public event ValueBehaviorDelegate<TValue> ValueUpdate;
        
        /// <summary>
        /// Событие на обновление данных. (Для адаптера)
        /// </summary>
        public event ValueBehaviorDelegate<TValue> ValueChange;
       
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
