using DynamicData;
using EquipApps.Mvc.Objects;
using System.Collections.Generic;

namespace EquipApps.Mvc.Services
{
    public class ActionService : IActionService
    {
        private readonly SourceCache<ActionDescriptor, TestNumber> sourceCache;

        public ActionService()
        {
            sourceCache = new SourceCache<ActionDescriptor, TestNumber>(x => x.Number);
            Observable = sourceCache.AsObservableCache();
        }

        public IObservableCache<ActionDescriptor, TestNumber> Observable { get; }

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
