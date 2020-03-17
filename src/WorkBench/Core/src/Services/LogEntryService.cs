using DynamicData;
using EquipApps.WorkBench.Models;
using System;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Services
{
    public class LogEntryService : ILogEntryService
    {
        private readonly SourceList<LogEntry> source;

        public LogEntryService()
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
