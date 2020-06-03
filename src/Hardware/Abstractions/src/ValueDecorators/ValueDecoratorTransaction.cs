using System;
using System.Transactions;

namespace EquipApps.Hardware.ValueDecorators
{
    /// <summary>
    /// Декоратор. 
    /// С поддержкой <see cref="IEnlistmentNotification"/>.
    /// </summary>
    public sealed class ValueDecoratorTransaction<TValue> : IValueComonent<TValue>, IEnlistmentNotification
    {
        IValueComonent<TValue> _valueComonent;

        private TValue _current
        {
            get => _valueComonent.Value;
            set => _valueComonent.Value = value;
        }       
        private volatile bool _enlisted;

        public ValueDecoratorTransaction(IValueComonent<TValue> valueComonent)
        {
            _valueComonent = valueComonent ?? throw new ArgumentNullException(nameof(valueComonent));

            Origin = _current;
            _enlisted = false;
        }

        public TValue Value
        {
            get => _current;
            set
            {
                //-- Изменяем текущий и оригинал если нет транзакции

                if (!Enlist())
                {
                    Origin = value;
                }
                _current = value;
            }
        }

        public TValue Origin
        {
            get;
            private set;
        }

        /// <summary>
        /// Подписаться на транзакцию
        /// </summary>       
        public bool Enlist()
        {
            if (_enlisted)
                //-- Идет транзакция
                return true;

            var currentTx = Transaction.Current;
            if (currentTx == null)
            {
                //-- Нет ранзакция
                return false;
            }

            //- Зарегистрировались
            currentTx.EnlistVolatile(this, EnlistmentOptions.None);
            _enlisted = true;

            return true;
        }

        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            //СОХРАНЯЕМ
            Origin = _current;
            _enlisted = false;

            enlistment.Done();
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            _enlisted = false;
        }

        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            //ПОДГОТОВКА ДАННЫХ К ИЗМЕНЕНИЮ
            preparingEnlistment.Prepared();
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            //ОТКАТ
            _current = Origin;
            _enlisted = false;
        }
    }
}
