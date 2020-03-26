using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Маркер привязки данных.
    /// Данные извлекаются из <see cref="IModelProvider{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BindModelAttribute : Attribute, IModelTypeMetadata, IBindingSourceMetadata
    {
        public BindModelAttribute(Type modelType)
        {
            ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
        }

        /// <summary>
        /// Возвращает тип модели
        /// </summary>
        public Type ModelType { get; }

        /// <summary>
        /// Возвращает источник <see cref="BindingSource.ModelProvider"/>
        /// </summary>
        public BindingSource BindingSource => BindingSource.ModelProvider;
    }
}
