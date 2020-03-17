﻿using DynamicData;
using EquipApps.WorkBench.Models;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Services
{
    public interface ILogEntryService
    {
        /// <summary>
        /// 
        /// </summary>
        IObservableList<LogEntry> Observable { get; }

        /// <summary>
        /// Поставить в очередь.
        /// </summary>
        void EnqueueEntry(LogEntry logEntry);

        /// <summary>
        /// Очистить
        /// </summary>
        void Clean();

        /// <summary>
        /// Очистить асинхронно
        /// </summary>
        Task CleanAsync();
    }
}
