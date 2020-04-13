using DynamicData;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Services
{
    public interface ILogService
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
