using DynamicData;
using EquipApps.Mvc.Reactive.LogsFeatures.Models;
using EquipApps.Mvc.Reactive.LogsFeatures.Services;
using EquipApps.Mvc.Viewers;
using Microsoft.Extensions.Options;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Viewers
{
    /// <summary>
    /// DependencyInjection (Transient).
    /// </summary>
    /// 
    public class LogViewer : IDisposable
    {
        private ReadOnlyObservableCollection<LogEntry> _Logs;
        private IDisposable itemsRefresher;

        public LogViewer(ILogService logService, IOptions<LogOptions> options)
        {
            Filter = new LogViewerFilter(options);

            //-- Подключаемя к данным
            var sourceConnect = logService.Observable.Connect();

            //-- Фильтруем данные
            var sourceFiltred = sourceConnect.Filter(Filter.ObservableFilter, ListFilterPolicy.ClearAndReplace);;

            CountTotal = new LogViewerCounter(sourceConnect);
            CountFiltr = new LogViewerCounter(sourceFiltred);

            //-- Отображаем данные
            itemsRefresher = sourceFiltred
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _Logs)
                .DisposeMany()
                .Subscribe();

            
        }

        /// <summary>
        /// Filter
        /// </summary>
        public LogViewerFilter Filter 
        { 
            get; 
        }

        /// <summary>
        /// 
        /// </summary>
        public LogViewerCounter CountTotal
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogViewerCounter CountFiltr
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
        public void Dispose()
        {
            itemsRefresher?.Dispose();
            itemsRefresher = null;

            CountFiltr.Dispose();
            CountTotal.Dispose();
            Filter.Dispose();
        }
    }
}
