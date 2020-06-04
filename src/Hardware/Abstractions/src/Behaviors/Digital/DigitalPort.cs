using EquipApps.Hardware.Abstractions;
using EquipApps.Hardware.ValueDecorators;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public class DigitalPort8   : ValueBehavior<byte>
    {
        public DigitalPort8()
        {
            InitializeLines(8);
        }
                
        protected virtual void InitializeLines(int count)
        {
            //-- Создаем массив.
            var list = new DigitalLine[count];

            for (int i = 0; i < count; i++)
            {
                //--
                var line = new DigitalLine(i);
                    line.ValueChange += Line_ValueChange;
                    line.ValueUpdate += Line_ValueUpdate;
                
                //--
                list[i] = line;
            }

            //-- Сохраняем.
            Lines = list;
        }

        public IReadOnlyList<DigitalLine> Lines 
        { 
            get; private set; 
        }

        private void Line_ValueUpdate(object sender, ValueBehaviorContext<Digit> context)
        {
            RequestToUpdateValue();
        }
        private void Line_ValueChange(object sender, ValueBehaviorContext<Digit> context)
        {
            //-- Линия
            var line  = sender as DigitalLine;

            //-- Новое значение для порта
            var value = context.Input == Digit.Nil
                ? ValueHelper.ToNil(Value, line.Index)
                : ValueHelper.ToOne(Value, line.Index);

            //-- Передаем дальше
            RequestToChangeValue(value);
        }

        public override void SetValue(byte value)
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

        protected override bool Enlist(TransactionType transactionType)
        {
            lock (locker)
            {
                //-- Идет транзакция
                if (_enlisted)
                {
                    //-- Идет транзакция тогоже типа!
                    if (_transaction == transactionType)
                        return true;
                    else
                        throw new InvalidOperationException(
                            $"Попытка начать новую транзакцию\n" +
                            $"Текущий тип транзакции {_transaction}" +
                            $"Новый тип транзакции {transactionType}");
                }


                //-- Извлекаем транзакцию. Если ее нет, то выходим.
                var currentTx = Transaction.Current;
                if (currentTx == null)
                {
                    return false;
                }

                //-- Зарегистрировались СИНХРОНИЗАЦИЮ!
                currentTx.EnlistVolatile(this, EnlistmentOptions.None);

                //-- Зарегистрировались ЛИНИЮ
                foreach (var line in Lines)
                {
                    line.Enlist();
                }



                _enlisted = true;
                _transaction = transactionType;

                return true;
            }
        }

    }

    public class DigitalPort16  : ValueBehavior<ushort>
    {
        public DigitalPort16()
        {
            InitializeLines(16);
        }

        protected virtual void InitializeLines(int count)
        {
            //-- Создаем массив.
            var list = new DigitalLine[count];

            for (int i = 0; i < count; i++)
            {
                //--
                var line = new DigitalLine(i);
                line.ValueChange += Line_ValueChange;
                line.ValueUpdate += Line_ValueUpdate;

                //--
                list[i] = line;
            }

            //-- Сохраняем.
            Lines = list;
        }

        public IReadOnlyList<DigitalLine> Lines
        {
            get; private set;
        }

        private void Line_ValueUpdate(object sender, ValueBehaviorContext<Digit> context)
        {
            RequestToUpdateValue();
        }
        private void Line_ValueChange(object sender, ValueBehaviorContext<Digit> context)
        {
            //-- Линия
            var line = sender as DigitalLine;

            //-- Новое значение для порта
            var value = context.Input == Digit.Nil
                ? ValueHelper.ToNil(Value, line.Index)
                : ValueHelper.ToOne(Value, line.Index);

            //-- Передаем дальше
            RequestToChangeValue(value);
        }

        public override void SetValue(ushort value)
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

        protected override bool Enlist(TransactionType transactionType)
        {
            lock (locker)
            {
                //-- Идет транзакция
                if (_enlisted)
                {
                    //-- Идет транзакция тогоже типа!
                    if (_transaction == transactionType)
                        return true;
                    else
                        throw new InvalidOperationException(
                            $"Попытка начать новую транзакцию\n" +
                            $"Текущий тип транзакции {_transaction}" +
                            $"Новый тип транзакции {transactionType}");
                }


                //-- Извлекаем транзакцию. Если ее нет, то выходим.
                var currentTx = Transaction.Current;
                if (currentTx == null)
                {
                    return false;
                }

                //-- Зарегистрировались СИНХРОНИЗАЦИЮ!
                currentTx.EnlistVolatile(this, EnlistmentOptions.None);

                //-- Зарегистрировались ЛИНИЮ
                foreach (var line in Lines)
                {
                    line.Enlist();
                }



                _enlisted = true;
                _transaction = transactionType;

                return true;
            }
        }
    }

}
