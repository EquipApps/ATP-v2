using EquipApps.Hardware.ValueDecorators;
using System;

namespace EquipApps.Hardware
{
    public class DigitalBehavior : ValueBehaviorBase<DigitalState>, IHardwareBehavior,
        IValueComonent<DigitalState>, IValueComonent<IHardware>
    {
        private ValueDecoratorObservable<DigitalState> valueObservable;
        private ValueDecoratorTransaction<DigitalState> valueTransaction;
        private ValueDecoratorObservable<IHardware> hardwareObservable;

        private DigitalState _state = DigitalState.Zed;
        private IHardware _hardware = null;

        public DigitalBehavior()
        {
            //---
            valueObservable = new ValueDecoratorObservable<DigitalState>(this);
            valueTransaction = new ValueDecoratorTransaction<DigitalState>(valueObservable);

            //---
            hardwareObservable = new ValueDecoratorObservable<IHardware>(this);
        }

        public override IHardware Hardware
        {
            get => hardwareObservable.Value;
            set => hardwareObservable.Value = value;
        }

        public override DigitalState Value
        {
            get => valueTransaction.Value;
            protected set => valueTransaction.Value = value;
        }

        //---
        public IObservable<IHardware> HardwareObservable => hardwareObservable.Observable;
        public IObservable<DigitalState> StateObservable => valueObservable.Observable;

        //---
        public override void Attach()
        {
        }

        //---
        DigitalState IValueComonent<DigitalState>.Value
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
