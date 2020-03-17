namespace EquipApps.Hardware.ValueDecorators
{
    /// <summary>
    /// Реализация компанента по умолчнию
    /// </summary>   
    public class ValueComonent<TValue> : IValueComonent<TValue>
    {

        public ValueComonent(TValue value)
        {
            Value = value;
        }

        public virtual TValue Value
        {
            get;
            set;
        }
    }
}
