using EquipApps.WorkBench.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class LogFilterScopeVM : ReactiveObject, IDisposable
    {
        private IDisposable _filterScopeListener;

        public LogFilterScopeVM()
        {
            ObservableFilter = this.WhenAnyValue(x => x.Scope, ObservedFilter);

            _filterScopeListener =
                MessageBus.Current.Listen<string>(MessageBusContracts.FilterScope)
                .ObserveOn  (RxApp.MainThreadScheduler)
                .BindTo     (this, x => x.Scope);
        }

        public IObservable<Func<LogEntry, bool>> ObservableFilter { get; }

        [Reactive]
        public string Scope { get; set; } = string.Empty;

        public void Dispose()
        {
            _filterScopeListener?.Dispose();
            _filterScopeListener = null;
        }


        private Func<LogEntry, bool> ObservedFilter(string filterScope)
        {
            if (string.IsNullOrEmpty(filterScope))
                return TRUE;

            return (LogEntry logEntrie) =>
            {
                return logEntrie.Scope == filterScope;
            };
        }

        private static bool TRUE(LogEntry logEntry)
        {
            return true;
        }
    }
}
