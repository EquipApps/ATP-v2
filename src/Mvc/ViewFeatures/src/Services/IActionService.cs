using DynamicData;
using System.Collections.Generic;

namespace EquipApps.Mvc.Services
{
    /// <summary>
    /// Singleton
    /// </summary>
    public interface IActionService
    {
        /// <summary>
        /// 
        /// </summary>
        IObservableCache<ActionObject, string> Observable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptors"></param>
        void Update(IEnumerable<ActionObject> actionDescriptors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        void Update(ActionObject actionDescriptor);

        /// <summary>
        /// 
        /// </summary>
        void Clear();
    }
}
