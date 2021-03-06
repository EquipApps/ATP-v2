﻿using DynamicData;
using DynamicData.Aggregation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Viewers
{
    /// <summary>
    /// Счетчик для <see cref="ActionDescriptor"/>.
    /// </summary>
    public class ActionsViewerCounter : ReactiveObject, IDisposable
    {
        private IDisposable _cleanUp;

        public ActionsViewerCounter(IObservable<IChangeSet<ActionObject, string>> source)
        {
            var totalDisposable
                = source.Count()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(value => Total = value);

            var passedDisposable
                = source.Filter(x => x.Result.Type == ActionObjectResultType.Passed)
                        .Count()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(value => Passed = value);

            var failedDisposable
                = source.Filter(x => x.Result.Type == ActionObjectResultType.Failed)
                        .Count()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(value => Failed = value);

            var notRunDisposable
                = source.Filter(x => x.Result.Type == ActionObjectResultType.NotRun)
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
