using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public class LogOptions
    {
        // <summary>
        /// Возвращает список ключ значение.
        /// Ключ     - Контекста.
        /// Значение - Имя группы.
        /// </summary>
        public Dictionary<string, string> ContextCollection { get; } = new Dictionary<string, string>();


        /// <summary>
        /// Возвращает список ключ значение.
        /// Ключ     - Имя группы.
        /// Значение - Информация о группе.
        /// </summary>
        public Dictionary<string, GroupInfo> GroupCollection { get; } = new Dictionary<string, GroupInfo>();
    }
}
