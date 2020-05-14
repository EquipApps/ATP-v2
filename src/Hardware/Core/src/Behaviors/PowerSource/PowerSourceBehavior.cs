using EquipApps.Hardware.ValueDecorators;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Hardware.Behaviors.PowerSource
{
    /// <summary>
    /// Поведение источника питания.
    /// </summary>
    public class PowerSourceBehavior : ValueBehaviorBase<PowerSourceState>,
         IValueComonent<PowerSourceState>
    {
        private readonly ValueDecoratorObservable<PowerSourceState> valueDecoratorObservable;
        private PowerSourceState _state;

        public PowerSourceBehavior()
        {
            valueDecoratorObservable = new ValueDecoratorObservable<PowerSourceState>(this);
            _state = PowerSourceState.OFF;
        }

        /// <inheritdoc/>
        public override IHardware Hardware { get; set; }

        /// <inheritdoc/>
        public override PowerSourceState Value 
        {
            get           => valueDecoratorObservable.Value;
            protected set => valueDecoratorObservable.Value = value;
        }

        /// <inheritdoc/>
        public override void Attach()
        {
           //-- Ничего не делаем.
        }


        PowerSourceState IValueComonent<PowerSourceState>.Value
        {
            get => _state;
            set => _state = value;
        }
    }
}
