using EquipApps.Hardware;
using EquipApps.Hardware.ValueDecorators;
using System;
using System.Transactions;

namespace EquipApps.WorkBench.Tools.External.Advantech
{
    /// <summary>
    /// 8-bit Digital input
    /// </summary>
    public class PCI_DI_Port : ValueBehaviorBase<byte>, IEnlistmentNotification,
        IValueComonent<byte>
    {
        private readonly ValueDecoratorTransaction<byte> valueTransaction;
        private volatile bool _enlisted;

        public PCI_DI_Port(byte number)
        {
            valueTransaction = new ValueDecoratorTransaction<byte>(this);

            Number = number;
            Lines = new PCI_DI_Line[8];

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new PCI_DI_Line();
                Lines[i].ValueUpdate += PCI_1762_Port_ValueUpdate;
            }

        }
        
        /// <summary>
        /// Номер порта
        /// </summary>
        public byte Number { get; }
                
        /// <summary>
        /// Лини.
        /// </summary>
        public PCI_DI_Line[] Lines { get; }

        /// <inheritdoc/> 
        public override byte Value
        {
            get           => valueTransaction.Value;
            protected set => valueTransaction.Value = value;
        }

        public override void SetValue(byte value)
        {
            //-- Устанавливаем значение Порта
            Value = value;

            //-- Устанавливаем значение Линии
            for (int i = 0; i < Lines.Length; i++)
            {
                var mask = 1 >> i;
                var data = value & mask;

                if (data == 0)
                {
                    Lines[i].SetValue(0);
                }
                else
                {
                    Lines[i].SetValue(1);
                }
            }
        }

        
        public override void RequestToUpdateValue()
        {
            /*
             * Проверяем идет ли транзакция.
             * Если идет, то подписвыаемся
             */

            if (Enlist())
            {
                //-- Идет транзакция.
            }
            else
            {

                base.RequestToUpdateValue();
            }
        }
        
        /// <summary>
        /// Запрос на обновление состояния порта
        /// </summary>
        /// <param name="valueBehavior"></param>
        private void PCI_1762_Port_ValueUpdate(ValueBehaviorBase<byte> valueBehavior)
        {
            RequestToUpdateValue();
        }

        /* 
         * Реализация IEnlistmentNotification
         * Позволяет организовать "синхронное" чтение данных из порта.
         * Уменьшает количестов низкоуровневых вызывов
         */

        #region EnlistmentRegion

        private bool Enlist()
        {
            if (_enlisted)
                //-- Идет транзакция
                return true;

            var currentTx = Transaction.Current;
            if (currentTx == null)
            {
                //-- Нет транзакция
                return false;
            }

            //-- Зарегистрировались СИНХРОНИЗАЦИЮ!
            currentTx.EnlistVolatile(this, EnlistmentOptions.None);

            //-- Зарегистрировались ПОРТ
            valueTransaction.Enlist();

            //-- Зарегистрировались ЛИНИЮ
            foreach (var line in Lines)
            {
                line.Enlist();
            }

            _enlisted = true;

            return true;
        }

        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            //-- Убираем флаг подписки      
            _enlisted = false;
            enlistment.Done();
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            //TODO: Что это?
            _enlisted = false;
        }

        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            //-- ПОДГОТОВКА ДАННЫХ К ИЗМЕНЕНИЮ
            try
            {
                base.RequestToUpdateValue();
                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                preparingEnlistment.ForceRollback(ex);
            }
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            //-- ОТКАТ            
        }

        byte IValueComonent<byte>.Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        #endregion
    }
}
