using EquipApps.Mvc.Reactive.LogsFeatures.Models;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Viewers
{
    public class LogViewerFilter : ReactiveObject, IDisposable
    {
        private IDisposable _filterScopeListener;

        private readonly Dictionary<string, GroupInfo> cache;
        private const double throttleMilliseconds = 100.0;

        public LogViewerFilter(IOptions<LogOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            cache = new Dictionary<string, GroupInfo>();

            foreach (var contextPair in options.Value.ContextCollection)
            {
                var context = contextPair.Key;
                var groupKey = contextPair.Value;

                if (options.Value.GroupCollection.TryGetValue(groupKey, out GroupInfo group))
                {
                    cache.Add(context, group);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            Groups = new ObservableCollection<GroupInfo>(options.Value.GroupCollection.Values);

            GroupSelected = Groups.FirstOrDefault();

            ObservableFilter = this.WhenAnyValue(x => x.ShowDbug,
                                                 x => x.ShowInfo,
                                                 x => x.ShowWarn,
                                                 x => x.ShowFail,
                                                 x => x.Scope,
                                                 x => x.GroupSelected,
                                                 ObservedFilter)
                                   .Throttle(TimeSpan.FromMilliseconds(throttleMilliseconds));

            _filterScopeListener = MessageBus.Current.Listen<string>(MessageBusContracts.FilterScope)
                                                     .ObserveOn(RxApp.MainThreadScheduler)
                                                     .BindTo(this, x => x.Scope);

            var canFilterRemove = this.WhenAnyValue(
                x => x.ShowDbug,
                x => x.ShowFail,
                x => x.ShowInfo,
                x => x.ShowWarn,
                x => x.Scope,
                (showDbug, showFail, showInfo, showWarn, scope) => showDbug || !showFail || !showInfo || !showWarn || !string.IsNullOrEmpty(scope));

            Clear = ReactiveCommand.Create(OnFilterRemove, canFilterRemove);

        }

        public IObservable<Func<LogEntry, bool>> ObservableFilter { get; }
        
        [Reactive]
        public bool ShowDbug { get; set; } = false;

        [Reactive]
        public bool ShowInfo { get; set; } = true;

        [Reactive]
        public bool ShowWarn { get; set; } = true;

        [Reactive]
        public bool ShowFail { get; set; } = true;

        [Reactive]
        public string Scope { get; set; } = string.Empty;


        [Reactive]
        public GroupInfo GroupSelected { get; set; }

        public ObservableCollection<GroupInfo> Groups { get; }

        public ReactiveCommand<Unit, Unit> Clear { get; }


        private Func<LogEntry, bool> ObservedFilter(bool showDbug,
                                                    bool showInfo,
                                                    bool showWarn,
                                                    bool showFail,
                                                    string scope,
                                                    GroupInfo groupInfo)
        {
            var filterGroup = GetFilter(groupInfo);
            var filterScope = GetFilter(scope);
            var filterLevel = GetFilter(showDbug, showInfo, showWarn, showFail);

            return (logEntrie) =>
            {
                return filterGroup(logEntrie) && filterScope(logEntrie) && filterLevel(logEntrie);
            };


        }


        private Func<LogEntry, bool> GetFilter(GroupInfo logGroupSelected)
        {
            return (logEntrie) =>
            {
                if (logEntrie == null)
                    return false;

                if (logGroupSelected == null)
                    return false;

                if (logGroupSelected.ShowManyContext)
                    return true;

                var logContext = logEntrie.Context;
                if (logContext == null)
                    return logGroupSelected.ShowNullContext;

                if (cache.TryGetValue(logContext, out GroupInfo logGroupForContext))
                    return logGroupSelected.Equals(logGroupForContext);

                return false;
            };
        }
        private Func<LogEntry, bool> GetFilter(string filterScope)
        {
            if (string.IsNullOrEmpty(filterScope))
                return TRUE;

            return (logEntrie) =>
            {
                return logEntrie.Scope == filterScope;
            };
        }
        private Func<LogEntry, bool> GetFilter(bool showDbug, bool showInfo, bool showWarn, bool showFail)
        {
            //TODO: Рефакторинг таблицы истинности!
            if (showDbug && showInfo && showWarn && showFail)
                return TRUE;

            if (!showDbug && !showInfo && !showWarn && !showFail)
                return FALSE;

            return (logEntrie) =>
            {
                if (logEntrie != null)
                {
                    switch (logEntrie.Level)
                    {
                        case LogEntryLevel.dbug: return showDbug;
                        case LogEntryLevel.info: return showInfo;
                        case LogEntryLevel.warn: return showWarn;
                        case LogEntryLevel.fail: return showFail;
                        default:
                            break;
                    }
                }

                return false;
            };
        }

        private void OnFilterRemove()
        {
            ShowDbug = false;
            ShowInfo = true;
            ShowWarn = true;
            ShowFail = true;
            Scope = string.Empty;
        }

        public static bool TRUE(LogEntry logEntry)
        {
            return true;
        }
        public static bool FALSE(LogEntry logEntry)
        {
            return false;
        }

        public void Dispose()
        {
            _filterScopeListener?.Dispose();
            _filterScopeListener = null;
        }
    }
}
