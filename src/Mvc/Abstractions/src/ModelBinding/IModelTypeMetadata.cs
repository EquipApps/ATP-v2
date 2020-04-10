using System;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Маркер.
    /// </summary>
    public interface IModelTypeMetadata
    {
        Type ModelType { get; }
    }
}
