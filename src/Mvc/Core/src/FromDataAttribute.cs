using EquipApps.Mvc.ModelBinding;
using System;

namespace EquipApps.Mvc
{
    // <summary>
    /// Настраивает привязку к свойству или параметру. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromDataAttribute : Attribute, IBindingSourceMetadata, IModelPathMetadata
    {
        /// <summary>
        /// Привязывает модель к свойству!
        /// </summary>
        public FromDataAttribute()
        {
            ModelPath = string.Empty;
        }

        public FromDataAttribute(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                throw new ArgumentNullException(nameof(modelName));
            }

            ModelPath = modelName;
        }

        /// <summary>
        /// Возвращает источник привязки <see cref="BindingSource.DataContext"/>
        /// </summary>
        public BindingSource BindingSource => BindingSource.DataContext;

        /// <summary>
        /// Возвращает / Устанавливает свойство привязки.  
        /// Если "" - привязывается источник.
        /// </summary>
        /// 
        /// <remarks>
        /// Формат:      
        ///     Property
        ///     Property.Property
        ///     Property[0]
        ///     Property[0].Property
        ///     Property[0].Property[0]       
        /// </remarks>
        /// 
        public string ModelPath { get; set; }


    }
}
