using DynamicData;
using System.Collections.Generic;

namespace EquipApps.Mvc.Services
{
    public class ActionService : IActionService
    {
        private readonly SourceCache<ActionObject, string> sourceCache;

        public ActionService()
        {
            sourceCache = new SourceCache<ActionObject, string>(x => x.Id);
            Observable = sourceCache.AsObservableCache();
        }

        public IObservableCache<ActionObject, string> Observable { get; }

        public void Update(IEnumerable<ActionObject> actions)
        {
            sourceCache.Edit(updater =>
            {                
                updater.Clear();
                updater.AddOrUpdate(actions);
            });
        }

        public void Update(ActionObject action)
        {
            sourceCache.AddOrUpdate(action);
        }

        public void Clear()
        {
            sourceCache.Clear();
        }

    }
}
