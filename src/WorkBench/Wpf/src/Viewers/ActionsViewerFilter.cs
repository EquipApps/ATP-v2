using EquipApps.Mvc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace EquipApps.WorkBench.Viewers
{
    public class ActionsViewerFilter : ReactiveObject
    {
        public ActionsViewerFilter()
        {
            ObservableFilter = this.WhenAnyValue(x => x.ShowPassed,
                                                 x => x.ShowFailed,
                                                 x => x.ShowNotRun,
                                                 UpdateFilter);
        }

        public IObservable<Func<ActionDescriptor, bool>> ObservableFilter { get; }


        [Reactive]
        public bool ShowPassed { get; set; } = true;

        [Reactive]
        public bool ShowFailed { get; set; } = true;

        [Reactive]
        public bool ShowNotRun { get; set; } = true;


        private Func<ActionDescriptor, bool> UpdateFilter(bool showPassed, bool showFailed, bool showNotRun)
        {
            return (ActionDescriptor actionDescriptor) =>
            {
                switch (actionDescriptor.Result)
                {
                    case Mvc.Abstractions.Result.NotRun:
                        return showNotRun;
                    case Mvc.Abstractions.Result.Passed:
                        return showPassed;
                    case Mvc.Abstractions.Result.Failed:
                        return showFailed;
                    default:
                        return false;
                }
            };
        }
    }
}
