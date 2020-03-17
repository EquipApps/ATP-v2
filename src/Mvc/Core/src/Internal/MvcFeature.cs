using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Internal
{
    /// <summary>
    /// Расширение.. 
    /// Доступно через <see cref="EquipApps.Testing.Features.IFeatureCollection"/>.
    /// Позволяет получить доступ к <see cref="ActionDescriptor"/>
    /// </summary>
    public class MvcFeature : IDisposable, IMvcFeature
    {
        public IReadOnlyList<ActionDescriptor> ActionDescriptors { get; set; }

        public void Dispose()
        {
            ActionDescriptors = null;
        }
    }
}
