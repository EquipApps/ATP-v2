using DynamicData;
using System;
using System.Reactive.Subjects;

namespace EquipApps.Mvc.Infrastructure
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
    public class LogLevelCounter : IObservable<LogLevelCount>, IDisposable
    {
        private IDisposable disposable;

        private volatile int countdbug = 0;
        private volatile int countinfo = 0;
        private volatile int countwarn = 0;
        private volatile int countfail = 0;

        private Subject<LogLevelCount> subject;

        public LogLevelCounter(IObservable<IChangeSet<LogEntry>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }



            subject = new Subject<LogLevelCount>();
            disposable = source.Subscribe(OnNext);
        }

        public int Сountdbug => countdbug;
        public int Сountinfo => countinfo;
        public int Сountwarn => countwarn;
        public int Сountfail => countfail;

        public IDisposable Subscribe(IObserver<LogLevelCount> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            //-- Передаем состояние новому подписчику
            //-- Необходимо чтобы у подписчика были актуальные данные.
            //-- (Проверка что подписка не багованная)

            observer.OnNext(new LogLevelCount
            {
                Countdbug = countdbug,
                Countinfo = countinfo,
                Countwarn = countwarn,
                Countfail = countfail,
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

            subject.OnNext(new LogLevelCount
            {
                Countdbug = countdbug,
                Countinfo = countinfo,
                Countwarn = countwarn,
                Countfail = countfail,
            });
        }

        private void OnNext(LogEntry logEntry)
        {
            switch (logEntry.Level)
            {
                case LogLevel.dbug: countdbug++; break;
                case LogLevel.info: countinfo++; break;
                case LogLevel.warn: countwarn++; break;
                case LogLevel.fail: countfail++; break;
                default:
                    break;
            }
        }

        private void Clear()
        {
            countdbug = 0;
            countinfo = 0;
            countwarn = 0;
            countfail = 0;
        }
    }
}
