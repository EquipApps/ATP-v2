namespace EquipApps.Hardware.ValueDecorators
{
    /// <summary>
    /// Компонент
    /// </summary>    
    public interface IValueComonent<TValue>
    {
        TValue Value { get; set; }
    }
}
