using System;
using System.Reactive.Subjects;
using System.Threading;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Infrastructure
{
    public class RuntimeLocker : IDisposable
    {
        private volatile ManualResetEvent threadLocker = new ManualResetEvent(false);
        private volatile RuntimeLockerCase runtimeCase = RuntimeLockerCase.Next;
        private readonly ISubject<bool> pauseChangedSubject = new ReplaySubject<bool>();


        /// <summary>
        /// 
        /// </summary>
        public IObservable<bool> ObservableLocker
        {
            get => pauseChangedSubject;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Next()
        {
            //--
            runtimeCase = RuntimeLockerCase.Next;
            //--
            threadLocker.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Replay()
        {
            //--
            runtimeCase = RuntimeLockerCase.Replay;
            //--
            threadLocker.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Previous()
        {
            //--
            runtimeCase = RuntimeLockerCase.Previous;
            //--
            threadLocker.Set();
        }

        /// <summary>
        /// Указывает можно ли повторить
        /// </summary>        
        public RuntimeLockerCase CaseAwite()
        {
            threadLocker.Reset();
            pauseChangedSubject.OnNext(true);

            threadLocker.WaitOne();
            pauseChangedSubject.OnNext(false);

            return runtimeCase;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            pauseChangedSubject.OnCompleted();
        }
    }
}
