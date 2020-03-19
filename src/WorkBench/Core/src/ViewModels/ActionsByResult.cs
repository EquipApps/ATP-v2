using DynamicData;
using DynamicData.Aggregation;
using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Objects;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionsByResult : ViewModelBase, IDisposable
    {
        private readonly IDisposable _cleanUp;

        public ActionsByResult(IGroup<ActionDescriptor, TestNumber, Result> group)
        {
            Result = group?.Key ?? throw new ArgumentNullException(nameof(group));

            _cleanUp = group.Cache.Connect()
                .Count()
                .Subscribe(count => Count = count);

        }

        public Result Result { get; }

        [Reactive] public int Count     { get; private set; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
