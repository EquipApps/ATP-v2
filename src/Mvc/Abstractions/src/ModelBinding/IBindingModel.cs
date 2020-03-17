namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Модель с привязкой данных
    /// </summary>
    public interface IBindingModel
    {
        /// <summary>
        /// Возвращает родительский <see cref="IBindingModel"/>
        /// </summary>
        IBindingModel Parent { get; }

        /// <summary>
        /// Задает или возвращает <see cref="ModelBinding.BindingInfo"/>.
        /// Может быть NULL.
        /// </summary>
        BindingInfo BindingInfo { get; }
    }
}
