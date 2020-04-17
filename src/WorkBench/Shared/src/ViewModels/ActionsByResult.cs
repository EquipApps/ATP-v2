using DynamicData;
using DynamicData.Aggregation;
using EquipApps.Mvc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionsByResult : ReactiveObject, IDisposable
    {
        private readonly IDisposable _cleanUp;

        public ActionsByResult(IGroup<ActionObject, string, ActionObjectResultType> group)
        {
            Result = group?.Key ?? throw new ArgumentNullException(nameof(group));

            _cleanUp = group.Cache.Connect()
                .Count()
                .Subscribe(count => Count = count);

        }

        public ActionObjectResultType Result { get; }

        [Reactive] public int Count     { get; private set; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
