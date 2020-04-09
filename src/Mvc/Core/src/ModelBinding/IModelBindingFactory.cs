namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктура фабрики <see cref="IModelBinder"/>.
    /// Singleton
    /// </summary>
    public interface IModelBindingFactory
    {
        /// <summary>
        /// Создает <see cref="IModelBinder"/>
        /// </summary>
        IModelBinder Create(IBindingModel bindingModel, BindingInfo bindingInfo);
    }
}
