using System;

namespace EquipApps.Mvc.Reactive.LogsFeatures.Models
{
    /// <summary>
    /// Запись в журнале
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Обединяет сообщения в логически связанные группы
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Контекст. Источник сообщений
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public LogEntryLevel Level { get; set; }

        /// <summary>
        /// Время поступления
        /// </summary>
        public DateTimeOffset Time { get; set; }

    }
}
