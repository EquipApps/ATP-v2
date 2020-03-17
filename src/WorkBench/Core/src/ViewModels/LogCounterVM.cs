using DynamicData;
using EquipApps.WorkBench.Models;
using EquipApps.WorkBench.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class LogCounterVM : ReactiveObject, IDisposable
    {
        private const double throttleMilliseconds = 500.0;
        private LogLevelCounter levelCounter;
        private IDisposable     disposable;

        public LogCounterVM(IObservable<IChangeSet<LogEntry>> source)
        {
            levelCounter = new LogLevelCounter(source);
            disposable   = levelCounter
                .Throttle   (TimeSpan.FromMilliseconds(throttleMilliseconds))
                .ObserveOn  (RxApp.MainThreadScheduler)
                .Subscribe  (OnNext);
        }

        public void Dispose()
        {
            disposable  .Dispose();
            levelCounter.Dispose();
        } 

        private void OnNext(LogLevelCount levelCount)
        {
            CountFail = levelCount.Countfail;
            CountInfo = levelCount.Countinfo;
            CountWarn = levelCount.Countwarn;
        }

        [Reactive]
        public int CountFail
        {
            get; private set;
        }

        [Reactive]
        public int CountInfo
        {
            get; private set;
        }

        [Reactive]
        public int CountWarn
        {
            get;
            private set;
        }
    }
}
