using DynamicData;
using System.Collections.Generic;

namespace EquipApps.Mvc.Services
{
    public class ActionService : IActionService
    {
        private readonly SourceCache<ActionDescriptor, Number> sourceCache;

        public ActionService()
        {
            sourceCache = new SourceCache<ActionDescriptor, Number>(x => x.Id);
            Observable = sourceCache.AsObservableCache();
        }

        public IObservableCache<ActionDescriptor, Number> Observable { get; }

        public void Update(IEnumerable<ActionDescriptor> actions)
        {
            sourceCache.Edit(updater =>
            {
                foreach (var item in updater.Items)
                {
                    item.Dispose();
                }

                updater.Clear();
                updater.AddOrUpdate(actions);
            });
        }

        public void Clear()
        {
            sourceCache.Edit(updater =>
            {
                foreach (var item in updater.Items)
                {
                    item.Dispose();
                }

                updater.Clear();
            });
        }
    }
}
