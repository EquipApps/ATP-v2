using EquipApps.Hardware.Abstractions;
using EquipApps.Hardware.ValueDecorators;
using System;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public class DigitalLine : IDigitBehavior, IValueComponent<Digit>, IDisposable
    {
        private readonly ValueDecoratorObservable <Digit>  valueDecoratorObservable;
        private readonly ValueDecoratorTransaction<Digit>  valueDecoratorTransaction;
        private Digit _value;

        /// <summary>
        /// 
        /// </summary>        
        public DigitalLine(IDigitalPort port, int index)
        {
            Index   = index;
            Port    = port ?? throw new ArgumentNullException(nameof(port));

            valueDecoratorObservable  = new ValueDecoratorObservable <Digit> (this);
            valueDecoratorTransaction = new ValueDecoratorTransaction<Digit> (valueDecoratorObservable);
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

        /// <summary>
        /// Digital Port Bit index; (Начинается с 0);
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Digital Port
        /// </summary>
        public IDigitalPort Port { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Digit Value
        {
            get           => valueDecoratorTransaction.Value;
            protected set => valueDecoratorTransaction.Value = value;
        }

        /// <summary>
        /// <para>Заппрос на изменение значения линии</para>
        /// <para>Передает запрос в порт. Через</para>
        /// </summary>        
        public void RequestToChangeValue(Digit value)
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

        /// <summary>
        /// Событие на изменение данных.  (Для адаптера)
        /// </summary> 
        public event Action<DigitalLine> ValueUpdate;

        /// <summary>
        /// Событие на обновление данных. (Для адаптера)
        /// </summary>
        public event Action<DigitalLine, Digit> ValueChange;
        
        /// <summary>
        /// Флаг.
        /// </summary>
        public bool CanUpdateValue => Port.CanUpdateValue;

        /// <summary>
        /// Флаг.
        /// </summary>
        public bool CanChangeValue => Port.CanChangeValue;

        /// <summary>
        /// Подписаться на транзакцию
        /// </summary>
        internal void Enlist()
        {
            valueDecoratorTransaction.Enlist();
        }

        /// <summary>
        /// Изменяет значение
        /// </summary>
        /// <param name="value"></param>
        internal void SetValue(Digit value)
        {
            Value = value;
        }

        public void Dispose()
        {
            valueDecoratorObservable.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        Digit IValueComponent<Digit>.Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
