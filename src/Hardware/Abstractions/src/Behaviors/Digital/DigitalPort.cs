using EquipApps.Hardware.Abstractions;
using EquipApps.Hardware.ValueDecorators;
using System;
using System.Collections.Generic;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public abstract class DigitalPort<TValue> : ValueBehavior<TValue>, IDigitalPort, IDisposable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Число линий в порту</param>
        public DigitalPort(int count)
        {
            //-- Создаем массив.
            var list = new DigitalLine[count];

            for (int i = 0; i < count; i++)
            {
                //--
                var line = new DigitalLine(this, i);
                    line.ValueChange += Line_ValueChange;
                    line.ValueUpdate += Line_ValueUpdate;
                //--
                list[i] = line;
            }

            //-- Сохраняем.
            Lines = list;
        }

        /// <summary>
        /// Обработка запроса на обновление состояния линии
        /// </summary>
        /// <param name="line">Цифровая линия</param>
        protected abstract void Line_ValueUpdate(DigitalLine line);

        /// <summary>
        /// Обработка запроса на изменение состояния линии
        /// </summary>
        /// <param name="line">Цифровая линия</param>
        /// <param name="digit">Цифровое значение</param>
        protected abstract void Line_ValueChange(DigitalLine line, Digit digit);

        /// <summary>
        /// Подписка на транзакцию линий
        /// </summary>
        protected override void Enlist()
        {
            base.Enlist();

            //-- Зарегистрировались ЛИНИЮ
            foreach (var line in Lines)
            {
                line.Enlist();
            }
        }

        /// <summary>
        /// Цифровые линии порта
        /// </summary>
        public IReadOnlyList<DigitalLine> Lines
        {
            get; private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            //-- Вызываем Dispose для каждой линии

            foreach (var line in Lines)
            {
                line.Dispose();
            }
        }
    }

    public class DigitalPort8   : DigitalPort<byte>
    {
        public DigitalPort8() :base(8)
        {
            
        }

        protected override void Line_ValueUpdate(DigitalLine line)
        {
            RequestToUpdateValue();
        }

        protected override void Line_ValueChange(DigitalLine line, Digit digit)
        {
            //-- Новое значение для порта
            var value = digit == Digit.Nil
                ? ValueHelper.ToNil(Value, line.Index)
                : ValueHelper.ToOne(Value, line.Index);

            //-- Передаем дальше
            RequestToChangeValue(value);
        }

        protected override void SetValue(byte value)
        {
            //-- Устанавливаем значение Порта
            Value = value;

            //-- Устанавливаем значение Линии
            for (int i = 0; i < Lines.Count; i++)
            {
                var digit = ValueHelper.GetDigit(value, i);

                Lines[i].SetValue(digit);
            }
        }

    }

    public class DigitalPort16  : DigitalPort<ushort>
    {
        public DigitalPort16() :base(16)
        {
           
        }

        protected override void Line_ValueUpdate(DigitalLine line)
        {
            RequestToUpdateValue();
        }

        protected override void Line_ValueChange(DigitalLine line, Digit digit)
        {
            //-- Новое значение для порта
            var value = digit == Digit.Nil
                ? ValueHelper.ToNil(Value, line.Index)
                : ValueHelper.ToOne(Value, line.Index);

            //-- Передаем дальше
            RequestToChangeValue(value);
        }

        protected override void SetValue(ushort value)
        {
            //-- Устанавливаем значение Порта
            Value = value;

            //-- Устанавливаем значение Линии
            for (int i = 0; i < Lines.Count; i++)
            {
                var digit = ValueHelper.GetDigit(value, i);

                Lines[i].SetValue(digit);
            }
        }

    }
}
