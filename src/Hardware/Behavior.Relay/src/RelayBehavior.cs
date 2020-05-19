using EquipApps.Hardware.ValueDecorators;
using System;

namespace EquipApps.Hardware
{
    public class RelayBehavior : ValueBehaviorBase<RelayState>, IHardwareBehavior,
        IValueComonent<RelayState>, IValueComonent<IHardware>, IRelayBehavior
    {
        private readonly ValueDecoratorObservable<RelayState> valueObservable;
        private readonly ValueDecoratorTransaction<RelayState> valueTransaction;
        private readonly ValueDecoratorObservable<IHardware> hardwareObservable;

        private RelayState _state = RelayState.Disconnect;
        private IHardware _hardware = null;

        public RelayBehavior()
        {
            //---
            valueObservable = new ValueDecoratorObservable<RelayState>(this);
            valueTransaction = new ValueDecoratorTransaction<RelayState>(valueObservable);

            //---
            hardwareObservable = new ValueDecoratorObservable<IHardware>(this);
        }

        //---
        public override IHardware Hardware
        {
            get => hardwareObservable.Value;
            set => hardwareObservable.Value = value;
        }
        public override RelayState Value
        {
            get => valueTransaction.Value;
            protected set => valueTransaction.Value = value;
        }

        //---
        public IObservable<IHardware> HardwareObservable => hardwareObservable.Observable;
        public IObservable<RelayState> StateObservable => valueObservable.Observable;

        //---
        public override void Attach()
        {
        }

        //---
        RelayState IValueComonent<RelayState>.Value
        {
            get => _state;
            set => _state = value;
        }
        IHardware IValueComonent<IHardware>.Value
        {
            get => _hardware;
            set => _hardware = value;
        }
    }
}
