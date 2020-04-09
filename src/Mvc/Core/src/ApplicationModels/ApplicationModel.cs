using System.Collections.Generic;
using System.Diagnostics;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель  приложения
    /// </summary>
    /// 
    [DebuggerDisplay("ApplicationModel: Controllers: {Controllers.Count}")]
    public class ApplicationModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ApplicationModel()
        {
            Controllers = new List<ControllerModel>();
            Properties = new Dictionary<object, object>();
        }

        /// <summary>
        /// Возвращает список <see cref="ControllerModel"/>
        /// </summary>
        public IList<ControllerModel> Controllers { get; }

        /// <summary>
        /// Gets a set of properties associated with all actions.
        /// These properties will be copied to <see cref="ActionDescriptor.Properties"/>.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}
