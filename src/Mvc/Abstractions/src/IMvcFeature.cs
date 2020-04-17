using EquipApps.Testing.Features;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Расширение.. 
    /// Доступно через <see cref="IFeatureCollection"/>.
    /// Позволяет получить доступ к <see cref="ActionDescriptor"/>.
    /// </summary>
    public interface IMvcFeature
    {
        /// <summary>
        /// Возвращает Список
        /// </summary>
        IReadOnlyList<ActionObject> ActionObjects { get; set; }
    }
}