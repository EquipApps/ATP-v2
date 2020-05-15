using DynamicData;
using EquipApps.Mvc.Reactive.LogsFeatures.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Viewers
{
    /// <summary>
    /// Счетчик.
    /// </summary>
    /// 
    /// <remarks>
    /// ВАЖНО!. 
    /// Работает только с вновь добавленными элементами коллекции
    /// Замена не поддерживается
    /// </remarks>
    /// 
    public class LogViewerCounter : ReactiveObject, IObservable<LogViewerCount>, IDisposable
    {
        private IDisposable disposable;
        private Subject<LogViewerCount> subject;

        public LogViewerCounter(IObservable<IChangeSet<LogEntry>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Сountdbug = 0;
            Сountinfo = 0;
            Сountwarn = 0;
            Сountfail = 0;

            subject = new Subject<LogViewerCount>();

            disposable = source.ObserveOn(RxApp.MainThreadScheduler)
                               .Subscribe(OnNext);
        }

        [Reactive]
        public int Сountdbug
        {
            get; private set;
        }
        [Reactive]
        public int Сountinfo
        {
            get; private set;
        }
        [Reactive]
        public int Сountwarn
        {
            get; private set;
        }
        [Reactive]
        public int Сountfail
        {
            get; private set;
        }

        public IDisposable Subscribe(IObserver<LogViewerCount> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            //-- Передаем состояние новому подписчику
            //-- Необходимо чтобы у подписчика были актуальные данные.
            //-- (Проверка что подписка не багованная)

            observer.OnNext(new LogViewerCount
            {
                Countdbug = Сountdbug,
                Countinfo = Сountinfo,
                Countwarn = Сountwarn,
                Countfail = Сountfail,
            });

            //-- Сохраняем подписчика
            return subject.Subscribe(observer);
        }

        public void Dispose()
        {
            disposable?.Dispose();
            subject?.Dispose();

            disposable = null;
            subject = null;
        }

        private void OnNext(IChangeSet<LogEntry> changes)
        {
            foreach (var change in changes)
            {
                switch (change.Reason)
                {
                    case ListChangeReason.Add:
                        {
                            OnNext(change.Item.Current);
                        }
                        break;
                    case ListChangeReason.AddRange:
                        {
                            foreach (var item in change.Range)
                            {
                                OnNext(item);
                            }
                        }
                        break;
                    case ListChangeReason.Clear:
                        {
                            Clear();
                        }
                        break;
                    case ListChangeReason.Replace:
                    case ListChangeReason.Remove:
                    case ListChangeReason.RemoveRange:
                    case ListChangeReason.Refresh:
                    case ListChangeReason.Moved:
                    default:
                        break;
                }
            }

            subject.OnNext(new LogViewerCount
            {
                Countdbug = Сountdbug,
                Countinfo = Сountinfo,
                Countwarn = Сountwarn,
                Countfail = Сountfail,
            });
        }

        private void OnNext(LogEntry logEntry)
        {
            switch (logEntry.Level)
            {
                case LogEntryLevel.dbug: Сountdbug++; break;
                case LogEntryLevel.info: Сountinfo++; break;
                case LogEntryLevel.warn: Сountwarn++; break;
                case LogEntryLevel.fail: Сountfail++; break;
                default:
                    break;
            }
        }

        private void Clear()
        {
            Сountdbug = 0;
            Сountinfo = 0;
            Сountwarn = 0;
            Сountfail = 0;
        }
    }
}
