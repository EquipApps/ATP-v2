using EquipApps.Hardware.Abstractions;

namespace EquipApps.Hardware.Behaviors.Toggling
{
    /// <summary>
    /// Маркер  для поведения тумблера
    /// </summary>
    public interface IToggleBehavior : IValueBehavior<Toggle>, IHardwareBehavior
    {
    }
}
