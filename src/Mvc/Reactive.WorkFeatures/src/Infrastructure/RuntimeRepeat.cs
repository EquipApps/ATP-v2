using System;
using System.Reactive;
using System.Reactive.Subjects;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure
{
    public class RuntimeRepeat : IDisposable
    {
        private readonly ISubject<Unit> countDownFinalSubject = new ReplaySubject<Unit>(1);
        private readonly object locker = new object();

        private volatile bool _isEnabled;
        private volatile bool _isEnabledCount;
        private volatile int  _count;

        public RuntimeRepeat()
        {
            _isEnabled = false;
            _isEnabledCount = false;
            _count = -1;
        }

        /// <summary>
        /// Включен или нет.
        /// </summary>
        public bool IsEnabled 
        {
            get => _isEnabled;
        }

        /// <summary>
        /// Позволяет подписаться на конец счетчика
        /// </summary>
        public IObservable<Unit> ObservableCountDownFinal
        {
            get => countDownFinalSubject;
        }

        /// <summary>
        /// Включает / выключает.
        /// </summary>
        public void SetCounter(int count = 0)
        {
            lock (locker)
            {
                switch (count)
                {
                    case -1:
                        {
                            _isEnabled = true;
                            _isEnabledCount = false;
                            break;
                        }
                    case 0:
                        {
                            _isEnabled = false;
                            _isEnabledCount = false;
                            break;
                        }
                    default:
                        {
                            _isEnabled = true;
                            _isEnabledCount = true;
                            break;
                        }    
                }
                _count = count;
            }
        }

        /// <summary>
        /// Включает / выключает.
        /// </summary>        
        public void Enabled(bool isEnbled)
        {
            if (isEnbled) 
                SetCounter(-1);
            else
                SetCounter(0);
        }

        /// <summary>
        /// Указывает можно ли повторить
        /// </summary>        
        public bool TryRepeat()
        {
            lock(locker)
            {
                //-- false - Если не включен
                if (!_isEnabled)
                {
                    return false;
                }

                //-- true - Если разрешен счетчик
                if (!_isEnabledCount)
                {
                    return true;
                }

                //-- true - Счктчик еще не сброшен

                if (_count-- != 0)
                {
                    return true;
                }

                //-- Изменяем состояние блокировщика!

                _isEnabled      = false;
                _isEnabledCount = false;
                _count          = -1;
            }

            /*
             * Оповещаем о конце обратного отсчета.
             * ВАЖНО: Не включать в lock(locker). 
             *        Для исключения ситуации взаимной блокировки
             */
            countDownFinalSubject.OnNext(Unit.Default);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            countDownFinalSubject.OnCompleted();
        }
    }
}
