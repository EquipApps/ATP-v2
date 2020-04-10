using EquipApps.Mvc.Abstractions;
using System;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктура связующей сущноти
    /// </summary>
    public interface IModelBinder
    {
        /// <summary>
        /// Привязать модель.
        /// ВАЖНО!!
        /// ПРИВЯЗКА НЕ ДОЛЖНА ВЫДОВАТЬ ИСКЛЮЧЕНИЯ!!
        /// </summary>
        [Obsolete("IHierarhicalDataObject")]
        BindingResult Bind(ActionDescriptorObject framworkElement, int offset = 0);
    }
}
