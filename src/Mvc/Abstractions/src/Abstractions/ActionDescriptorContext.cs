using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Контекс. Используется в <see cref="IActionProvider"/> для создания <see cref="ActionDescriptor"/>
    /// </summary>
    public class ActionDescriptorContext
    {
        public List<ActionDescriptor> Results { get; set; } = new List<ActionDescriptor>();
    }
}
