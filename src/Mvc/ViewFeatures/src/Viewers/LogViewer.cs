using DynamicData;
using EquipApps.Mvc.Services;
using Microsoft.Extensions.Options;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Viewers
{
    public partial class LogViewer : IDisposable
    {
        private const double throttleMilliseconds = 500.0;        
        private ReadOnlyObservableCollection<LogEntry> _Logs;
        private IDisposable itemsRefresher;

        public LogViewer(ILogService logService, IOptions<LogOptions> options)
        {
            FilterGroup = new LogViewerFilterGroup(options?.Value);
            FilterScope = new LogViewerFilterScope();
            FilterLevel = new LogViewerFilterLevel();

            //-- НАБЛЮДАТЕЛЬ ЗА ИЗМЕНЕННИЕМ ФИЛЬТРА!
            //-- не чаще чем 500мс
            var observableFilter = this.WhenAnyObservable(x => x.FilterGroup.ObservableFilter,
                                                      x => x.FilterScope.ObservableFilter,
                                                      x => x.FilterLevel.ObservableFilter,
                                                      ObservedFilter)
                                   .Throttle(TimeSpan.FromMilliseconds(throttleMilliseconds));

            //-- Подключаемя к данным
            var sourceConnect = logService.Observable.Connect();

            //-- Фильтруем данные
            var sourceFiltred = sourceConnect
                .Filter(FilterGroup.ObservableFilter, ListFilterPolicy.CalculateDiff)
                .Filter(FilterScope.ObservableFilter, ListFilterPolicy.CalculateDiff)
                .Filter(FilterLevel.ObservableFilter, ListFilterPolicy.CalculateDiff);

            //-- Отображаем данные
            itemsRefresher = sourceFiltred
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _Logs)
                .DisposeMany()
                .Subscribe();



            TotalCount    = new LogViewerCounter(sourceConnect);
            FiltrCount  = new LogViewerCounter(sourceFiltred);



            var canFilterRemove = this.WhenAnyValue(
                x => x.FilterLevel.ShowDbug,
                x => x.FilterLevel.ShowFail,
                x => x.FilterLevel.ShowInfo,
                x => x.FilterLevel.ShowWarn,
                x => x.FilterScope.Scope,
                (showDbug, showFail, showInfo, showWarn, scope) => showDbug || !showFail || !showInfo || !showWarn || !string.IsNullOrEmpty(scope));

            FilterRemove = ReactiveCommand.Create(OnFilterRemove, canFilterRemove);
        }

        private Func<LogEntry, bool> ObservedFilter(Func<LogEntry, bool> filterGroup,
                                                    Func<LogEntry, bool> filterScope,
                                                    Func<LogEntry, bool> filterLevel)
        {
            if (filterGroup == null)
                throw new ArgumentNullException(nameof(filterScope));

            if (filterScope == null)
                throw new ArgumentNullException(nameof(filterLevel));

            if (filterLevel == null)
                throw new ArgumentNullException(nameof(filterLevel));


            return (logEntrie) =>
            {
                return filterGroup(logEntrie) && filterScope(logEntrie) && filterLevel(logEntrie);
            };
        }


        /// <summary>
        /// Group Filter
        /// </summary>
        public LogViewerFilterGroup FilterGroup
        {
            get;
        }

        /// <summary>
        /// Level Filter
        /// </summary>
        public LogViewerFilterLevel FilterLevel
        {
            get;
        }

        /// <summary>
        /// Scope Filter
        /// </summary>
        public LogViewerFilterScope FilterScope
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogViewerCounter TotalCount
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogViewerCounter FiltrCount
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyObservableCollection<LogEntry> Logs => _Logs;

        /// <summary>
        /// 
        /// </summary>
        public ReactiveCommand<Unit, Unit> FilterRemove { get;}

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            itemsRefresher?.Dispose();
            itemsRefresher = null;

            FiltrCount.Dispose();
            TotalCount.Dispose(); 

            //FilterGroup.Dispose();
            //FilterLevel.Dispose();
            FilterScope.Dispose();
          
        }



        private void OnFilterRemove()
        {
            FilterLevel.ShowDbug = false;
            FilterLevel.ShowInfo = true;
            FilterLevel.ShowWarn = true;
            FilterLevel.ShowFail = true;
            FilterScope.Scope    = string.Empty;
        }
    }
}
