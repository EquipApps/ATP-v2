using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Infrastructure
{
    public class MvcFeature : IMvcFeature, IDisposable
    {
        /// <inheritdoc/>
        public IReadOnlyList<ActionObject> ActionObjects { get; set; }

        /*
         * Вызывает Dispose для каждого ActionObject
         * Данный метод вызывается во премя вызова ITest.Dispose
         * 
         */
        public void Dispose()
        {
            var actionObjects = ActionObjects;
            if (actionObjects != null)
            {
                foreach (var actionObject in actionObjects)
                {
                    actionObject.Dispose();
                }
            }

            
            ActionObjects = null;
        }
    }
}
