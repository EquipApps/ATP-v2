using EquipApps.Mvc.ModelBinding;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктура фабрики <see cref="IModelBinder"/>.
    /// Singleton
    /// </summary>
    public interface IBindingFactory
    {
        /// <summary>
        /// Создает <see cref="IModelBinder"/>
        /// </summary>
        IModelBinder Create(IBindingModel bindingModel, BindingInfo bindingInfo);
    }
}
