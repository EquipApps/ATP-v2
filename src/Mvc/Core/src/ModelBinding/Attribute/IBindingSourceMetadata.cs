using EquipApps.Mvc.ModelBinding;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Attribute
{
    /// <summary>
    /// Маркер.
    /// </summary>
    public interface IBindingSourceMetadata
    {
        BindingSource BindingSource { get; }
    }
}