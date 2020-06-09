namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Делегат. Используется для обработки <see cref="ValueBehaviorContext{TValue}"/>
    /// </summary>    
    public delegate void ValueBehaviorDelegate<TValue>(object sender, ValueBehaviorContext<TValue> context);
}
