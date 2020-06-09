using EquipApps.Hardware.Abstractions;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public sealed class DigitalLine : ValueLine<Digit>, IDigitBehavior
    {
        /// <summary>
        /// 
        /// </summary>        
        public DigitalLine(IValueBehavior port, int index)
            : base(port, index)
        {

        }

        /// <summary>
        /// Подписаться на транзакцию
        /// </summary>
        internal void Enlist()
        {
            valueDecoratorTransaction.Enlist();
        }

        internal void SetValue(Digit value)
        {
            Value = value;
        }
    }
}
