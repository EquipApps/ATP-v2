using System;

namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Содержит информацию о поведении
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class ValueBehaviorContext<TValue>
    {
        private TValue output;

        public ValueBehaviorContext(TValue input)
        {
            Input       = input;
            output      = default(TValue);
            IsHandled   = false;
        }

        /// <summary>
        /// Флаг. Поведение обработано.
        /// </summary>
        public bool IsHandled { get; private set; }
        
        /// <summary>
        /// Возвращает входное значение. (Используется для изменения состояния)
        /// </summary>
        public TValue Input { get; }


        /// <summary>
        /// Устанавливает результат обработки
        /// </summary>       
        public void SetOutput(TValue value)
        {
            if (IsHandled)
            {
                throw new InvalidOperationException("Output has already been set");
            }
            else
            {
                IsHandled = true;
                output = value;
            }
        }

        /// <summary>
        /// Возвращает результат обработки
        /// </summary> 
        public TValue GetOutput()
        {
            if (IsHandled)
            {
                return output;
            }
            else
            {
                throw new InvalidOperationException("Output has not been set");
            }
        }
    }
}
