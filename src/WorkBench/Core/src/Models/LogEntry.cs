using System;

namespace EquipApps.WorkBench.Models
{
    /// <summary>
    /// Запись в журнале
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Задает или возвращает
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Возвращает контекст.
        /// </summary>
        public string Context { get; set; }




        public LogLevel  LogLevel { get; set; }





        public DateTimeOffset Time { get; set; }
        public string Message { get; set; }


        

        
    }
}
