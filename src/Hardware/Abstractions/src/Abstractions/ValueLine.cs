using EquipApps.Hardware.ValueDecorators;
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
    ///  |A|                    |B|                 |L| <---- (Request)     |E| 1) Расширение отправляет запросы через поведение
    ///  |D|                    |E| <---- (Event)   |I|                     |X| 2) Поведение отовещает адаптер через событие
    ///  |A| <---- (Event)      |H|                 |N|                     |T| 3) Адаптер обрабатывает событие 
    ///  |P|  ---> (Context)    |A|                 |E|                     |E| и устанавливает результат обработки через контекст события
    ///  |T|                    |V| ----> (Setter)  | |                     |N| 4) Поведение анализирует состояние контекста и изменяет состояние буферного значения Value
    ///  |E|                    |I|                 | |  ----> (Value)      |T| 5) Расширение анализирует значение Value
    ///  |R|                    |O|                 | |                     |I|
    ///                         |R|                                         |O|
    ///                                                                     |N|
    /// 
    /// </remarks>
    /// 
    public abstract class ValueLine<TValue> : IValueBehavior<TValue>, IValueComponent<TValue>, IDisposable
    {
        protected readonly ValueDecoratorObservable   <TValue> valueDecoratorObservable;
        protected readonly ValueDecoratorTransaction  <TValue> valueDecoratorTransaction;
        private TValue  _value;
         

        public ValueLine(IValueBehavior owner,int index)
        {
            if(index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Index = index;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));

            valueDecoratorObservable  = new ValueDecoratorObservable    <TValue>(this);
            valueDecoratorTransaction = new ValueDecoratorTransaction   <TValue>(valueDecoratorObservable);
        }

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

        #region IValueBehavior

        /// <summary>
        /// Возвращает значение линии
        /// </summary>
        public TValue Value
        {
            get           => valueDecoratorTransaction.Value;
            protected set => valueDecoratorTransaction.Value = value;
        }

        /// <summary>
        /// <para>Заппрос на изменение значения линии</para>
        /// <para>Передает запрос в порт. Через</para>
        /// </summary>        
        public void RequestToChangeValue(TValue value)
        {
            var valueChange = ValueChange;
            if (valueChange == null)
                throw new InvalidOperationException("Запрос на изменение значения не поддерживается");
                valueChange.Invoke(this, value);
        }

        /// <summary>
        /// <para>Заппрос на обновление значения линии</para>
        /// <para>Передает запрос в порт. Через</para>
        /// </summary>
        public void RequestToUpdateValue()
        {
            var valueUpdate = ValueUpdate;
            if (valueUpdate == null)
                throw new InvalidOperationException("Запрос на обновление значения не поддерживается");
                valueUpdate.Invoke(this);
        }

        /// <inheritdoc/>
        public bool CanUpdateValue => Owner.CanUpdateValue;

        /// <inheritdoc/>
        public bool CanChangeValue => Owner.CanChangeValue;


        #endregion

        /// <summary>
        /// Возвращает индекс линии; (Начинается с 0);
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Возвращает владельца линии
        /// </summary>
        public IValueBehavior Owner { get; }

        /// <summary>
        /// Событие на изменение данных. (Для обработки владельцем)
        /// </summary>   
        public event Action<ValueLine<TValue>> ValueUpdate;

        /// <summary>
        /// Событие на обновление данных. (Для обработки владельцем)
        /// </summary>
        public event Action<ValueLine<TValue>,TValue> ValueChange;

        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        TValue IValueComponent<TValue>.Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
