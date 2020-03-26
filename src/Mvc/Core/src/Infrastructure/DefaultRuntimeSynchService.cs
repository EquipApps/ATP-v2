using System;
using System.Reactive.Subjects;
using System.Threading;

namespace EquipApps.Mvc.Runtime
{
    public enum RuntimeCase
    {
        Previous,
        Replay,
        Next,
    }

    public class DefaultRuntimeSynchService : IRuntimeService, IDisposable
    {
        private volatile ManualResetEvent threadLocker = new ManualResetEvent(false);
        private volatile RuntimeCase runtimeCase = RuntimeCase.Next;
        

        private readonly ISubject<bool> pauseChangedSubject = new ReplaySubject<bool>();
        private volatile bool isEnabledPause = false;



        public int MillisecondsTimeout { get; set; }



        public bool IsEnabledRepeat { get; set; }
        public bool IsEnabledRepeatOnce { get; set; }

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

        public IObservable<bool> ObservablePause => pauseChangedSubject;

        public void Next()      
        {
            //--
            runtimeCase = RuntimeCase.Next;
            //--
            threadLocker.Set();
        }

        public void Previous()  
        {
            //--
            runtimeCase = RuntimeCase.Previous;
            //--
            threadLocker.Set();
        }

        public void Replay()    
        {
            //--
            runtimeCase = RuntimeCase.Replay;
            //--
            threadLocker.Set();
        }

        public void Dispose()   
        {
            throw new NotImplementedException();
        }
        
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

        internal void Repeat()
        {
            //TODO: Обновить через настройки
            Thread.Sleep(100);
        }
    }
}
