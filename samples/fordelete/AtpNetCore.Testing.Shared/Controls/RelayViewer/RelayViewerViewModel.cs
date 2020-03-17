using DynamicData;
using EquipApps.Hardware;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.Controls.RelayViewer
{
    public partial class RelayViewerViewModel : ReactiveObject
    {
        IHardwareCollection _hardwareCollection;

        private readonly ReadOnlyObservableCollection<RelayViewModel> _items;

        public RelayViewerViewModel(IHardwareCollection hardwareCollection)
        {
            _hardwareCollection = hardwareCollection ?? throw new ArgumentNullException(nameof(hardwareCollection));

            _hardwareCollection
                .Connect()
                .AutoRefreshOnObservable(x => x.Behaviors.Connect())
                .Filter(FilterByRelayBehavior)
                .Transform(x => new RelayViewModel(x.Behaviors.Get<RelayBehavior>()))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .DisposeMany()
                .Subscribe();
        }

        public ReadOnlyObservableCollection<RelayViewModel> Items
        {
            get
            {
                return _items;
            }
        }
        
        private bool FilterByRelayBehavior(IHardware hardware)
        {
            if (hardware == null)
                return false;

            return hardware.Behaviors.ContainsBehaviorWithKey<RelayBehavior>();
        }
    }
}
