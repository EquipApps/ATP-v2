using EquipApps.Mvc.ModelBinding.Property;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Контекст провайдера привязок
    /// </summary>
    public abstract class BinderProviderContext
    {
        /// <summary>
        /// Возвращает <see cref="IModelMetadataProvider"/>
        /// </summary>
        public abstract IModelMetadataProvider MetadataProvider { get; }

        /// <summary>
        /// Возвращает <see cref="IPropertyProvider"/>
        /// </summary>
        public abstract IPropertyProvider PropertyProvider { get; }

        /// <summary>
        /// Возвращает <see cref="IBindingModel"/>.
        /// </summary>
        public abstract IBindingModel BindingModel { get; }

        /// <summary>
        /// Возвращает <see cref="ModelBinding.BindingInfo"/>
        /// </summary>
        public abstract BindingInfo BindingInfo { get; }

        /// <summary>
        /// Создает <see cref="IModelBinder"/>
        /// </summary>
        public abstract IModelBinder CreateBinder(ModelMetadata property);

    }
}
