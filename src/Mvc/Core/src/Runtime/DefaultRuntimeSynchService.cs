using System;
using System.Reactive.Subjects;
using System.Threading;

namespace EquipApps.Mvc.Runtime
{
    public class DefaultRuntimeSynchService : IRuntimeService, IDisposable
    {
        private volatile ManualResetEvent threadLocker = new ManualResetEvent(false);
        private volatile RuntimeCase runtimeCase = RuntimeCase.Next;
        

        private readonly ISubject<bool> pauseChangedSubject = new ReplaySubject<bool>();
        private volatile bool isEnabledPause = false;

        public DefaultRuntimeSynchService()
        {
            MillisecondsTimeout = 100;
            CountRepeat = -1;
        }

        /// <inheritdoc/>
        public bool IsEnabledRepeat
        { 
            get; 
            set; 
        }
        
        /// <inheritdoc/>
        public bool IsEnabledRepeatOnce
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool IsEnabledPause
        {
            get => isEnabledPause;
            set
            {
                var oldValue = isEnabledPause;
                if (oldValue)
                {
                    //--
                    runtimeCase = RuntimeCase.Next;
                    //--
                    threadLocker.Set();
                }

                isEnabledPause = value;
            }
        }

        /// <inheritdoc/>
        public int MillisecondsTimeout
        {
            get;
            set;
        }

        public int CountRepeat
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public IObservable<bool> ObservablePause => pauseChangedSubject;

        /// <inheritdoc/>
        public void Next()      
        {
            //--
            runtimeCase = RuntimeCase.Next;
            //--
            threadLocker.Set();
        }

        /// <inheritdoc/>
        public void Replay()    
        {
            //--
            runtimeCase = RuntimeCase.Replay;
            //--
            threadLocker.Set();
        }

        /// <inheritdoc/>
        public void Previous()  
        {
            //--
            runtimeCase = RuntimeCase.Previous;
            //--
            threadLocker.Set();
        }

        #region internal

        /// <summary>
        /// Конвеер переходт в состояние паузы
        /// </summary>
        /// <returns></returns>
        internal RuntimeCase Pause()
        {
            threadLocker.Reset();
            pauseChangedSubject.OnNext(true);
           
            threadLocker.WaitOne();
            pauseChangedSubject.OnNext(false);

            return runtimeCase;
        }

        internal void RepeatOnce()
        {
            //TODO: Обновить через настройки
            Thread.Sleep(100);
        }

        public bool TryRepeat()
        {
            return false;
        }

        internal void Repeat()
        {
            var count = CountRepeat;
            if (count < 0)
            {
                Sleep();
            }

            if (count > 0) 
            { 

            }


            
            
        }

        internal void Sleep()
        {
            var timeOut = MillisecondsTimeout;
            if (timeOut > 0)
            {
                Thread.Sleep(timeOut);
            }
        }

        #endregion

        public void Dispose()
        {
            //TODO: Написать юнит тест
            pauseChangedSubject.OnCompleted();
        }
    }
}
