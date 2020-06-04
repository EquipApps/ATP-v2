using EquipApps.Hardware.Abstractions;

namespace EquipApps.Hardware.Behaviors.Digital
{
    /// <summary>
    /// Поведение цифровой линии.
    /// </summary>
    public interface IDigitBehavior : IValueBehavior<Digit>, IHardwareBehavior
    {
    }
}