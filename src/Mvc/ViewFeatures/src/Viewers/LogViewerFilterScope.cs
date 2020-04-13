using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Viewers
{
    public class LogViewerFilterScope : ReactiveObject, IDisposable
    {
        private IDisposable _filterScopeListener;

        public LogViewerFilterScope()
        {
            ObservableFilter = this.WhenAnyValue(x => x.Scope, ObservedFilter);

            _filterScopeListener =
                MessageBus.Current.Listen<string>(MessageBusContracts.FilterScope)
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(this, x => x.Scope);
        }

        public IObservable<Func<LogEntry, bool>> ObservableFilter { get; }

        [Reactive] public string Scope { get; set; } = string.Empty;

        private Func<LogEntry, bool> ObservedFilter(string filterScope)
        {
            if (string.IsNullOrEmpty(filterScope))
                return LogViewerFilter.TRUE;

            return (logEntrie) =>
            {
                return logEntrie.Scope == filterScope;
            };
        }

        public void Dispose()
        {
            _filterScopeListener?.Dispose();
            _filterScopeListener = null;
        }
    }
}
