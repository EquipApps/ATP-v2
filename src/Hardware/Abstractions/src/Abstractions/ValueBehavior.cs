using EquipApps.Hardware.ValueDecorators;
using System;
using System.Diagnostics;
using System.Transactions;

namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Реализация поведения с поддержкой транзакции.
    /// </summary>    
    public abstract class ValueBehavior<TValue> : ValueBehaviorBase<TValue>,
        IEnlistmentNotification, IValueComonent<TValue>, IDisposable, IUnhandledBehavior
    {
        protected volatile object locker = new object();
        protected volatile bool _enlisted = false;
        protected volatile bool _prepare = false;
        protected volatile TransactionType _transaction;

        private readonly ValueDecoratorObservable<TValue> valueDecoratorObservable;
        private readonly ValueDecoratorTransaction<TValue> valueDecoratorTransaction;

        private TValue _value;

        public ValueBehavior()
        {
            valueDecoratorObservable = new ValueDecoratorObservable<TValue>(this);
            valueDecoratorTransaction = new ValueDecoratorTransaction<TValue>(valueDecoratorObservable);
        }

        public override TValue Value
        {
            get           => valueDecoratorTransaction.Value;
            protected set => valueDecoratorTransaction.Value = value;
        }

        public override void RequestToChangeValue(TValue value)
        {
            if (Enlist(TransactionType.RequestToChange))
            {
                //-- Мы подписались на транзакцию.
                //-- Сохраняем в буфер новое значение!
                Value = value;
            }
            else
            {
                base.RequestToChangeValue(value);
            }
        }

        public override void RequestToUpdateValue()
        {
            if (Enlist(TransactionType.RequestToUpdate))
            {
                //-- Мы подписались на транзакцию.
            }
            else
            {
                base.RequestToUpdateValue();
            }
        }

        ///<inheritdoc/>           
        public event UnhandledExceptionEventHandler UnhandledExceptionEvent;

        public IObservable<TValue> ObservableValue => valueDecoratorObservable.Observable;

        #region EnlistmentRegion

        protected virtual bool Enlist(TransactionType transactionType)
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

                _enlisted = true;
                _transaction = transactionType;

                return true;
            }
        }

        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            lock (locker)
            {
                _enlisted = false;
                _transaction = TransactionType.empty;
                _prepare = false;
            }

            enlistment.Done();
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            lock (locker)
            {
                _enlisted = false;
                _transaction = TransactionType.empty;
                _prepare = false;
            }
        }

        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            //-- ПОДГОТОВКА ДАННЫХ К ИЗМЕНЕНИЮ
            try
            {
                switch (_transaction)
                {
                    case TransactionType.RequestToChange:
                        {
                            base.RequestToChangeValue(Value);
                            break;
                        }
                    case TransactionType.RequestToUpdate:
                        {
                            base.RequestToUpdateValue();
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_transaction));
                }

                _prepare = true;

                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                preparingEnlistment.ForceRollback(ex);
            }
        }

        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            try
            {
                if (_prepare)
                {
                    switch (_transaction)
                    {
                        case TransactionType.RequestToChange:
                            {
                                base.RequestToChangeValue(valueDecoratorTransaction.Origin);
                                break;
                            }
                        case TransactionType.RequestToUpdate:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(_transaction));
                    }
                }
            }
            catch (Exception ex)
            {
                UnhandledException(ex);
            }
            finally
            {
                lock (locker)
                {
                    _enlisted = false;
                    _transaction = TransactionType.empty;
                    _prepare = false;
                }

                enlistment.Done();
            }
        }

        private void UnhandledException(Exception ex)
        {
            try
            {
                //-- Проблема при откате системы!
                var eventUE = UnhandledExceptionEvent;
                if (eventUE != null)
                {
                    eventUE.Invoke(this, new UnhandledExceptionEventArgs(ex, true));
                }
            }
            catch (Exception exCall)
            {
                Debug.Fail("UnhandledException", exCall.ToString());
            }
        }

        #endregion

        TValue IValueComonent<TValue>.Value
        {
            get => _value;
            set => _value = value;
        }

        protected enum TransactionType
        {
            empty,
            RequestToChange,
            RequestToUpdate
        }

        public void Dispose()
        {
            valueDecoratorObservable.Dispose();
        }
    }
}
