using EquipApps.Hardware.Abstractions;
using EquipApps.Hardware.ValueDecorators;
using System;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public class DigitalLine : ValueBehaviorBase<Digit>, IDigitBehavior,
        IValueComonent<Digit>
    {
        private readonly ValueDecoratorObservable<Digit>  valueDecoratorObservable;
        private readonly ValueDecoratorTransaction<Digit> valueDecoratorTransaction;

        private Digit _value;

        public DigitalLine(int index)
        {
            Index = index;

            valueDecoratorObservable  = new ValueDecoratorObservable<Digit> (this);
            valueDecoratorTransaction = new ValueDecoratorTransaction<Digit>(valueDecoratorObservable);
        }

        public int Index { get; }

        public IObservable<Digit> ObservableValue => valueDecoratorObservable.Observable;

        internal void Enlist()
        {
            valueDecoratorTransaction.Enlist();
        }

        public override Digit Value 
        {
            get           => valueDecoratorTransaction.Value;
            protected set => valueDecoratorTransaction.Value = value;
        }

        Digit IValueComonent<Digit>.Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
