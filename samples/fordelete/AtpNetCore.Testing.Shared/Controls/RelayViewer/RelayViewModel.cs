using EquipApps.Hardware;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Controls.RelayViewer
{
    public partial class RelayViewModel : ReactiveObject, IDisposable
    {
        private RelayBehavior   _behavior;
        private IDisposable     _cleanUp;

        public RelayViewModel(RelayBehavior behavior)
        {
            _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));

            Updater (_behavior.Value);
            Updater (_behavior.Hardware);

            var nameRefresher = _behavior.HardwareObservable.Subscribe(Updater);
            var valuRefresher = _behavior.StateObservable   .Subscribe(Updater);

            _cleanUp = Disposable.Create(() =>
            {
                nameRefresher.Dispose();
                valuRefresher.Dispose();
            });
        }

        private void Updater(RelayState value)
        {
            Value = value == RelayState.Connect;
        }

        private void Updater(IHardware hardware)
        {
            Name = hardware?.Name ?? "Unknow";
        }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public bool Value { get; set; }






        public void Dispose()
        {
            _cleanUp?.Dispose();
        }
    }
}
