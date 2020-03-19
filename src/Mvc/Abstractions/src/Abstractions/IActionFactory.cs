using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Singleton
    /// </summary>
    public interface IActionFactory
    {
        IReadOnlyList<ActionDescriptor> GetActionDescriptors();
    }
}
