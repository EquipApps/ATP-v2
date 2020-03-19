using System.Collections.Generic;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Расширение.. 
    /// Доступно через <see cref="Testing.Features.IFeatureCollection"/>.
    /// Позволяет получить доступ к <see cref="ActionDescriptor"/>
    /// </summary>
    public interface IMvcFeature
    {
        IReadOnlyList<ActionDescriptor> ActionDescriptors { get; set; }
    }
}