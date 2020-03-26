using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.Controllers
{
    /// <summary>
    /// Функция приложения, Возвращает список типов контроллеров приложения
    /// </summary>
    public class ControllerFeature
    {
        /// <summary>
        /// Возвращает список типов контроллеров приложения
        /// </summary>
        public IList<TypeInfo> Controllers { get; } = new List<TypeInfo>();
    }
}
