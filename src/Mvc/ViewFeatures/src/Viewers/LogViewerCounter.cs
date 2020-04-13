using DynamicData;
using EquipApps.Mvc.Infrastructure;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Viewers
{
    public class LogViewerCounter : ReactiveObject, IDisposable
    {
        private const double throttleMilliseconds = 500.0;
        private LogLevelCounter levelCounter;
        private IDisposable disposable;

        public LogViewerCounter(IObservable<IChangeSet<LogEntry>> source)
        {
            levelCounter = new LogLevelCounter(source);
            disposable = levelCounter
                .Throttle(TimeSpan.FromMilliseconds(throttleMilliseconds))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(OnNext);
        }

        public void Dispose()
        {
            disposable.Dispose();
            levelCounter.Dispose();
        }

        private void OnNext(LogLevelCount levelCount)
        {
            CountFail = levelCount.Countfail;
            CountInfo = levelCount.Countinfo;
            CountWarn = levelCount.Countwarn;
        }

        [Reactive] public int CountFail
        {
            get; private set;
        }

        [Reactive] public int CountInfo
        {
            get; private set;
        }

        [Reactive] public int CountWarn
        {
            get;
            private set;
        }
    }
}
