using EquipApps.Hardware.Abstractions;
using EquipApps.Hardware.ValueDecorators;
using System;

namespace EquipApps.Hardware
{
    public class DigitalBehavior : ValueBehaviorBase<byte>,  IHardwareBehavior, IDigitalLineBehavior,
        IValueComonent<byte>, IValueComonent<IHardware>
    {
        private ValueDecoratorObservable<byte> valueObservable;
        private ValueDecoratorTransaction<byte> valueTransaction;
        private ValueDecoratorObservable<IHardware> hardwareObservable;

        private byte _state = 0;
        private IHardware _hardware = null;

        public DigitalBehavior()
        {
            //---
            valueObservable = new ValueDecoratorObservable<byte>(this);
            valueTransaction = new ValueDecoratorTransaction<byte>(valueObservable);

            //---
            hardwareObservable = new ValueDecoratorObservable<IHardware>(this);
        }

        public override IHardware Hardware
        {
            get => hardwareObservable.Value;
            set => hardwareObservable.Value = value;
        }

        public override byte Value
        {
            get => valueTransaction.Value;
            protected set => valueTransaction.Value = value;
        }

        //---
        public IObservable<IHardware> HardwareObservable => hardwareObservable.Observable;
        public IObservable<byte> StateObservable => valueObservable.Observable;

        //---
        public override void Attach()
        {
        }

        //---
        byte IValueComonent<byte>.Value
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
