using DynamicData;
using EquipApps.Mvc.Objects;
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
        IObservableCache<ActionDescriptor, TestNumber> Observable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptors"></param>
        void Update(IEnumerable<ActionDescriptor> actionDescriptors);

        /// <summary>
        /// 
        /// </summary>
        void Clear();
    }
}
