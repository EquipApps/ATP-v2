namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Инфроструктура объекта поведения значения. 
    /// (Используется для построения иерархии связанных поведений)
    /// </summary>
    public interface IValueBehavior : IHardwareBehavior
    {
        /// <summary>
        /// Флаг. Можно ли обновлять значение
        /// </summary>
        bool CanUpdateValue { get; }

        /// <summary>
        /// Флаг. Можно ли изменять значение
        /// </summary>
        bool CanChangeValue { get; }
    }
}
