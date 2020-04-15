﻿using DynamicData;
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
        IObservableCache<ActionDescriptor, string> Observable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptors"></param>
        void Update(IEnumerable<ActionDescriptor> actionDescriptors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        void Update(ActionDescriptor actionDescriptor);

        /// <summary>
        /// 
        /// </summary>
        void Clear();
    }
}
