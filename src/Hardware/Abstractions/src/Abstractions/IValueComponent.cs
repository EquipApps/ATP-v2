namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Компонент
    /// </summary>    
    public interface IValueComponent<TValue>
    {
        TValue Value { get; set; }
    }
}
