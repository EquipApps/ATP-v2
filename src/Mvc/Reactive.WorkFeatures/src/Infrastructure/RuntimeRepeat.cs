using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Subjects;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure
{
    public class RuntimeRepeat : IDisposable
    {
        private readonly ISubject<int> countSubject = new ReplaySubject<int>(1);
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
        public IObservable<int> ObservableCount
        {
            get => countSubject;
        }

        /// <summary>
        /// Включает / выключает.
        /// </summary>
        public void SetCounter(int count = 0)
        {
            //-- Ограничитель
            if (count < 0) count = -1;

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

                countSubject.OnNext(_count);
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

                //-- true - Если не включен счетчик
                if (!_isEnabledCount)
                {
                    return true;
                }

                /*
                 * Проверка счетчика. Отладка.
                 */
                Debug.Assert(_count >= 0);

                //-- true - Счетчик еще не сброшен
                if (_count-- == 0)
                {
                    //-- Изменяем состояние блокировщика!
                    _isEnabled      = false;
                    _isEnabledCount = false;
                   
                    countSubject.OnNext(_count);

                    return false;
                }
                else
                {
                    countSubject.OnNext(_count);

                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            countSubject.OnCompleted();
        }
    }
}
