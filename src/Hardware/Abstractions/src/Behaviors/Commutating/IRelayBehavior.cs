using EquipApps.Hardware.Abstractions;

namespace EquipApps.Hardware.Behaviors.Commutating
{
    /// <summary>
    /// Маркер  для поведения реле
    /// </summary>
    public interface IRelayBehavior : IValueBehavior<Relay>, IHardwareBehavior
    {

    }
}