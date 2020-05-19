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
        private TValue _original;
        private bool _enlisted;

        public ValueDecoratorTransaction(IValueComonent<TValue> valueComonent)
        {
            _valueComonent = valueComonent ?? throw new ArgumentNullException(nameof(valueComonent));

            _original = _current;
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
                    _original = value;
                }
                _current = value;
            }
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
            _original = _current;
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
            //ПОДГОТОВКА ДАННЫХ К ИЗМЕНЕНИЮ
            preparingEnlistment.Prepared();
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            //ОТКАТ
            _current = _original;
            _enlisted = false;
        }
    }
}
