using EquipApps.Hardware.ValueDecorators;
using System;
using System.Diagnostics;
using System.Transactions;

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
    public abstract class ValueBehavior<TValue> : IValueBehavior<TValue>, IEnlistmentNotification,
        IValueComponent<TValue>, IUnhandledComponent, 
        IDisposable
    {
        private volatile object locker  = new object();
        private volatile bool _enlisted = false;
        private volatile bool _prepare  = false;
        private volatile TransactionType _transaction;

        private readonly ValueDecoratorObservable <TValue> valueDecoratorObservable;
        private readonly ValueDecoratorTransaction<TValue> valueDecoratorTransaction;
        private TValue _value;

        public ValueBehavior()
        {
            valueDecoratorObservable  = new ValueDecoratorObservable<TValue>(this);
            valueDecoratorTransaction = new ValueDecoratorTransaction<TValue>(valueDecoratorObservable);
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

        ///<inheritdoc/>
        public TValue Value
        {
            get           => valueDecoratorTransaction.Value;
            protected set => valueDecoratorTransaction.Value = value;
        }

        ///<inheritdoc/>
        public void RequestToChangeValue(TValue value)
        {
            if (Enlist(TransactionType.RequestToChange))
            {
                //-- Мы подписались на транзакцию.
                //-- Сохраняем в буфер новое значение!
                Value = value;
            }
            else
            {
                ValueChangeCall(value);
            }
        }

        ///<inheritdoc/>
        public void RequestToUpdateValue()
        {
            if (Enlist(TransactionType.RequestToUpdate))
            {
                //-- Мы подписались на транзакцию.
            }
            else
            {
                ValueUpdateCall();
            }
        }

        /// <inheritdoc/>    
        public bool CanUpdateValue => ValueUpdate != null;

        /// <inheritdoc/>    
        public bool CanChangeValue => ValueChange != null;

        #endregion

        /// <summary>
        /// Событие на изменение данных. (Для обработки адаптером)
        /// </summary>   
        public event ValueBehaviorDelegate<TValue> ValueUpdate;

        /// <summary>
        /// Событие на обновление данных. (Для обработки адаптером)
        /// </summary>
        public event ValueBehaviorDelegate<TValue> ValueChange;

        ///<inheritdoc/>           
        public event UnhandledExceptionEventHandler UnhandledExceptionEvent;

        /// <summary>
        /// Изменяет <see cref="Value"/>
        /// </summary>
        protected virtual void SetValue(TValue value)
        {
            Value = value;
        }
        
        private void ValueUpdateCall()            
        {
            var valueUpdate = ValueUpdate;
            if (valueUpdate == null)
                throw new InvalidOperationException("Запрос на обновление значения не поддерживается");

            var context = new ValueBehaviorContext<TValue>(Value);

            valueUpdate.Invoke(this, context);

            var output = context.GetOutput();

            SetValue(output);
        }

        private void ValueChangeCall(TValue value)
        {
            var valueChange = ValueChange;
            if (valueChange == null)
                throw new InvalidOperationException("Запрос на изменение значения не поддерживается");

            var context = new ValueBehaviorContext<TValue>(value);

            valueChange.Invoke(this, context);

            var output = context.GetOutput();

            SetValue(output);
        }

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
                          EnlistDecorator();

                _enlisted    = true;
                _transaction = transactionType;

                return true;
            }
        }

        protected virtual void EnlistDecorator()
        {
            //-- Подписаться на транзакцию
            valueDecoratorTransaction.Enlist();
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
                            ValueChangeCall(Value);
                            break;
                        }
                    case TransactionType.RequestToUpdate:
                        {
                            ValueUpdateCall();
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
                                ValueChangeCall(valueDecoratorTransaction.Origin);
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

        ///<inheritdoc/>
        public virtual void Dispose()
        {
            valueDecoratorObservable.Dispose();
        }

        /// <inheritdoc/>
        TValue IValueComponent<TValue>.Value
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
    }
}
