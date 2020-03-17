using DynamicData;
using EquipApps.WorkBench.Models;
using EquipApps.WorkBench.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class LogViewerViewModel : IDisposable
    {
        private const double throttleMilliseconds = 500.0;

        private IObservable<Func<LogEntry, bool>> observableFilter;
        private ReadOnlyObservableCollection<LogEntry> _Logs;
        private IDisposable itemsRefresher;

        private void ctor(ILogEntryService logEntriesService, LogOptions logOptions)
        {
            if (logEntriesService == null)
                throw new ArgumentNullException(nameof(logEntriesService));

            FilterGroup = new LogFilterGroupVM(logOptions);
            FilterLevel = new LogFilterLevelVM();
            FilterScope = new LogFilterScopeVM();
            

            //-- НАБЛЮДАТЕЛЬ ЗА ИЗМЕНЕННИЕМ ФИЛЬТРА!
            //-- не чаще чем 500мс
            observableFilter = this.WhenAnyObservable(x => x.FilterGroup.ObservableFilter,
                                                      x => x.FilterScope.ObservableFilter,
                                                      x => x.FilterLevel.ObservableFilter,
                                                      ObservedFilter)
                                   .Throttle(TimeSpan.FromMilliseconds(throttleMilliseconds));

            var sourceConnext = logEntriesService.Observable.Connect();
            var sourceFiltred = sourceConnext.Filter(observableFilter, ListFilterPolicy.ClearAndReplace);

            TotlalCount = new LogCounterVM(sourceConnext);
            LevelCount  = new LogCounterVM(sourceFiltred);

            itemsRefresher = sourceFiltred
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _Logs)
                .DisposeMany()
                .Subscribe();


            var canFilterRemove = this.WhenAnyValue(                
                x => x.FilterLevel.ShowDbug,
                x => x.FilterLevel.ShowFail,
                x => x.FilterLevel.ShowInfo,
                x => x.FilterLevel.ShowWarn,
                x => x.FilterScope.Scope,
               
                (showDbug, showFail, showInfo, showWarn, scope) => showDbug || !showFail || !showInfo || !showWarn || !string.IsNullOrEmpty(scope));


            FilterRemove = ReactiveCommand.Create(OnFilterRemove, canFilterRemove);
        }

        private Func<LogEntry, bool> ObservedFilter(Func<LogEntry, bool> filterGroup, Func<LogEntry, bool> filterScope, Func<LogEntry, bool> filterLevel)
        {
            if (filterGroup == null)
                throw new ArgumentNullException(nameof(filterScope));

            if (filterScope == null)
                throw new ArgumentNullException(nameof(filterLevel));

            if (filterLevel == null)
                throw new ArgumentNullException(nameof(filterLevel));


            return (LogEntry logEntrie) =>
            {
                return filterGroup(logEntrie) && filterScope(logEntrie) && filterLevel(logEntrie);
            };
        }


        /// <summary>
        /// Group Filter
        /// </summary>
        [Reactive] public LogFilterGroupVM FilterGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// Level Filter
        /// </summary>
        [Reactive] public LogFilterLevelVM FilterLevel 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Scope Filter
        /// </summary>
        [Reactive] public LogFilterScopeVM FilterScope
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public LogCounterVM TotlalCount 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public LogCounterVM LevelCount 
        { 
            get; private set; 
        }

        public ReadOnlyObservableCollection<LogEntry> Logs => _Logs;

        public ICommand FilterRemove { get; private set; }

        public void Dispose()
        {
            itemsRefresher?.Dispose();
            itemsRefresher = null;

            LevelCount?.Dispose();
            LevelCount = null;

            TotlalCount?.Dispose();
            TotlalCount = null;

            observableFilter = null;

            FilterScope?.Dispose();
            FilterScope = null;

            FilterLevel = null;
        }

       

        private void OnFilterRemove()
        {
            this.FilterLevel.ShowDbug = false;
            this.FilterLevel.ShowInfo = true;
            this.FilterLevel.ShowWarn = true;
            this.FilterLevel.ShowFail = true;

            this.FilterScope.Scope = string.Empty;
        }
    }
}
