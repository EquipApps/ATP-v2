using DynamicData;
using EquipApps.Mvc.Reactive.LogsFeatures.Models;
using System;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Services
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// </summary>
    public class LogService : ILogService
    {
        private readonly SourceList<LogEntry> source;

        public LogService()
        {
            source = new SourceList<LogEntry>();
            Observable = source.AsObservableList();
        }

        public IObservableList<LogEntry> Observable { get; }

        public void Clean()
        {
            source.Clear();
            GC.Collect();
        }

        public Task CleanAsync()
        {
            return Task.Run(Clean);
        }

        public void EnqueueEntry(LogEntry logEntry)
        {
            if (logEntry == null)
            {
                throw new ArgumentNullException(nameof(logEntry));
            }

            source.Add(logEntry);
        }
    }
}
