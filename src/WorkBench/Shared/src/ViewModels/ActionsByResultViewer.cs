using DynamicData;
using DynamicData.Binding;
using EquipApps.Mvc.Services;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionsByResultViewer : ReactiveObject, IDisposable
    {
        private readonly IDisposable _cleanUp;
        private readonly ReadOnlyObservableCollection<ActionsByResult> _data;

        public ActionsByResultViewer(IActionService actionService)
        {
            if (actionService == null)
            {
                throw new ArgumentNullException(nameof(actionService));
            }

            var grouperRefresher = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => Unit.Default);

            _cleanUp        = actionService.Observable.Connect()
                .Group      (action => action.Result.Type, grouperRefresher)
                .Transform  (group  => new ActionsByResult(group))
                .Sort       (SortExpressionComparer<ActionsByResult>.Descending(t => t.Result))
                .ObserveOn  (RxApp.MainThreadScheduler)
                .Bind       (out _data)
                .DisposeMany()
                .Subscribe  ();
        }

        public ReadOnlyObservableCollection<ActionsByResult> Data => _data;

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
