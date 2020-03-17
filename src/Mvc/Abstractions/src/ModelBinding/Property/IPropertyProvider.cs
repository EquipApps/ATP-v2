using System;

namespace EquipApps.Mvc.ModelBinding.Property
{
    /// <summary>
    /// Инфроструктура провайдера свойств
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        /// Извлекает свойство <see cref="PropertyEntery"/> если это возможно.
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="properyPath"></param>
        /// <param name="propertyEntery"></param>
        /// <returns></returns>
        bool TryGetModelProperty(Type sourceType, string properyPath, out PropertyEntery propertyEntery);
    }
}
