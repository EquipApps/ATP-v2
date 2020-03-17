using EquipApps.Mvc.ModelBinding.Metadata;
using EquipApps.Mvc.ModelBinding.Property;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Контекст провайдера привязок
    /// </summary>
    public abstract class BinderProviderContext
    {
        /// <summary>
        /// Возвращает <see cref="IMetadataProvider"/>
        /// </summary>
        public abstract IMetadataProvider MetadataProvider { get; }

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
        /// Создает <see cref="IBinder"/>
        /// </summary>
        public abstract IBinder CreateBinder(ModelMetadata property);

    }
}
