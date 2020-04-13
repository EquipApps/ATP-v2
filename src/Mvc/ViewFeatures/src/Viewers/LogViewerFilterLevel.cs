using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace EquipApps.Mvc.Viewers
{
    public class LogViewerFilterLevel : ReactiveObject
    {
        public LogViewerFilterLevel()
        {
            ObservableFilter = this.WhenAnyValue(x => x.ShowDbug,
                                                 x => x.ShowInfo,
                                                 x => x.ShowWarn,
                                                 x => x.ShowFail,
                                                 ObservedFilter);
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

        private Func<LogEntry, bool> ObservedFilter(bool showDbug, bool showInfo, bool showWarn, bool showFail)
        {
            //TODO: Рефакторинг таблицы истинности!
            if (showDbug && showInfo && showWarn && showFail)
                return LogViewerFilter.TRUE;

            if (!showDbug && !showInfo && !showWarn && !showFail)
                return LogViewerFilter.FALSE;

            return (LogEntry logEntrie) =>
            {
                if (logEntrie != null)
                {
                    switch (logEntrie.Level)
                    {
                        case LogLevel.dbug: return showDbug;
                        case LogLevel.info: return showInfo;
                        case LogLevel.warn: return showWarn;
                        case LogLevel.fail: return showFail;
                        default:
                            break;
                    }
                }

                return false;
            };
        }
    }


}
