using DynamicData;
using EquipApps.Hardware;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.Controls.BatteryViewer
{
    /// <summary>
    /// Модель представление источников питания
    /// Находит все устройства с поддержкой мониторинга напряжения питания
    /// </summary>
    public class BatteryViewerViewModel : ReactiveObject
    {
        private readonly IHardwareCollection _hardwareCollection;
        private readonly ReadOnlyObservableCollection<BatteryViewModel> _items;

        public BatteryViewerViewModel(IHardwareCollection hardwareCollection)
        {
            _hardwareCollection = hardwareCollection ?? throw new ArgumentNullException(nameof(hardwareCollection));
            _hardwareCollection
                .Connect()
                .AutoRefreshOnObservable(x => x.Behaviors.Connect())
                .Filter(FilterByBatteryBehavior)
                .Transform(x => new BatteryViewModel(x.Behaviors.Get<BatteryBehavior>()))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .DisposeMany()
                .Subscribe();

        }

        public ReadOnlyObservableCollection<BatteryViewModel> Items
        {
            get
            {
                return _items;
            }
        }
        
        private bool FilterByBatteryBehavior(IHardware hardware)
        {
            if    (hardware == null)
                return false;

            return hardware.Behaviors.ContainsBehaviorWithKey<BatteryBehavior>();
        }
    }
}
