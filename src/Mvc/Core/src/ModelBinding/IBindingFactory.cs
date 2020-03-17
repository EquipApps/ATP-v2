using EquipApps.Mvc.ModelBinding;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктура фабрики <see cref="IBinder"/>.
    /// Singleton
    /// </summary>
    public interface IBindingFactory
    {
        /// <summary>
        /// Создает <see cref="IBinder"/>
        /// </summary>
        IBinder Create(IBindingModel bindingModel, BindingInfo bindingInfo);
    }
}
