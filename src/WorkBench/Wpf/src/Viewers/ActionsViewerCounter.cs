using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Alias;
using EquipApps.Mvc.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Viewers
{
    public class ActionsViewerCounter : ReactiveObject, IDisposable
    {
        private const double throttleMilliseconds = 500.0;
        private IDisposable _cleanUp;

        public ActionsViewerCounter(IObservable<IChangeSet<ActionDescriptor, string>> source)
        {
            var totalDisposable = source.Count()
                                        .ObserveOn(RxApp.MainThreadScheduler)
                                        .Subscribe(value => Total = value);

            var passedDisposable = source.Filter(x => x.Result == Result.Passed)
                                         .Count()
                                         .ObserveOn(RxApp.MainThreadScheduler)
                                         .Subscribe(value => Passed = value);

            var failedDisposable = source.Filter(x => x.Result == Result.Failed)
                                         .Count()
                                         .ObserveOn(RxApp.MainThreadScheduler)
                                         .Subscribe(value => Failed = value);

            var notRunDisposable = source.Filter(x => x.Result == Result.NotRun)
                                         .Count()
                                         .ObserveOn(RxApp.MainThreadScheduler)
                                         .Subscribe(value => NotRun = value);

            _cleanUp = new CompositeDisposable(totalDisposable,
                                               passedDisposable,
                                               failedDisposable,
                                               notRunDisposable);
        }

        /// <summary>
        /// Всего тестов
        /// </summary>
        [Reactive] public int Total { get; private set; }
        
        /// <summary>
        /// Счетчик. Пройденных тестов
        /// </summary>
        [Reactive] public int Passed { get; private set; }

        /// <summary>
        /// Счетчик. заваленных тестов
        /// </summary>
        [Reactive] public int Failed { get; private set; }

        /// <summary>
        /// Счетчик. не выполненных тестов
        /// </summary>
        [Reactive] public int NotRun { get; private set; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
