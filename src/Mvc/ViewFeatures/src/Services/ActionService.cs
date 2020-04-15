using DynamicData;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Services
{
    public class ActionService : IActionService
    {
        private readonly SourceCache<ActionDescriptor, string> sourceCache;

        public ActionService()
        {
            sourceCache = new SourceCache<ActionDescriptor, string>(x => x.Id);
            Observable = sourceCache.AsObservableCache();
        }

        public IObservableCache<ActionDescriptor, string> Observable { get; }

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

        public void Update(ActionDescriptor actionDescriptor)
        {
            sourceCache.AddOrUpdate(actionDescriptor);
            //sourceCache.Edit(updater =>
            //{
            //    updater.Refresh(actionDescriptor);
            //});
        }

        public void Update()
        {
            sourceCache.Edit(updater =>
            {
                updater.AddOrUpdate(updater.Items);
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
