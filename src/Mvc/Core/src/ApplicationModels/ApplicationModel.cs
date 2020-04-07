using System.Collections.Generic;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель  приложения
    /// </summary>
    public class ApplicationModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ApplicationModel()
        {
            Areas = new List<AreaModel>();
            Controllers = new List<ControllerModel>();
        }

        /// <summary>
        /// Возвращает список <see cref="AreaModel"/>.
        /// </summary>
        public IList<AreaModel> Areas { get; }

        /// <summary>
        /// Возвращает список <see cref="ControllerModel"/>
        /// </summary>
        public IList<ControllerModel> Controllers { get; }
    }
}
