using EquipApps.Hardware.ValueDecorators;

namespace EquipApps.Hardware.Behaviors.Toggling
{
    /// <summary>
    /// Поведение тумблера.
    /// </summary>
    /// 
    /// <remarks>
    /// Не ссылаться на прямую. Работать через интерфейс.
    /// </remarks>
    public class ToggleBehavior : ValueBehaviorBase<Toggle>,
         IValueComonent<Toggle>
    {
        private readonly ValueDecoratorObservable<Toggle> valueDecoratorObservable;
        private Toggle _state;

        public ToggleBehavior()
        {
            valueDecoratorObservable = new ValueDecoratorObservable<Toggle>(this);
            _state = Toggle.OFF;
        }

        /// <inheritdoc/>
        public override Toggle Value 
        {
            get           => valueDecoratorObservable.Value;
            protected set => valueDecoratorObservable.Value = value;
        }


        Toggle IValueComonent<Toggle>.Value
        {
            get => _state;
            set => _state = value;
        }
    }
}
