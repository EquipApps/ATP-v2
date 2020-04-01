using EquipApps.Mvc.ModelBinding;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Attribute
{
    /// <summary>
    /// Маркер.
    /// </summary>
    public interface IBinderTypeMetadata : IBindingSourceMetadata
    {
        Type BinderType { get; }
    }
}