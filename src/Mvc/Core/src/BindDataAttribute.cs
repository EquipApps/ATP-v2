using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Маркер привязки данных.
    /// Данные извлекаются из DataContext.
    /// </summary>    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BindDataAttribute : Attribute, IModelTypeMetadata, IModelPathMetadata, IBindingSourceMetadata
    {
        /// <summary>
        /// Конструктор явной привязки
        /// </summary>
        /// 
        /// <param name="modelType">
        /// Указывает тип модели, которую привязываем к действию
        /// </param>
        /// 
        /// <param name="modelPath">
        /// Указывает путь извлечения модели
        /// </param>
        /// 
        /// <remarks>
        /// Фича: Если указать путь к коллекции, а тип как элемент коллекции,
        ///       то данные будут извлекаться из коллекции!       
        /// </remarks>
        public BindDataAttribute(Type modelType, string modelPath)
        {
            ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
            ModelPath = modelPath ?? throw new ArgumentNullException(nameof(modelPath));
        }

        /// <summary>
        /// Конструктор не явной привязки
        /// </summary>
        /// 
        /// <param name="modelName">
        /// Указывает путь извлечения модели
        /// </param>
        /// 
        /// <remarks>
        /// Провайдер привязки должен сам определить тип модели
        /// </remarks>
        public BindDataAttribute(string modelName)
        {
            ModelPath = modelName ?? throw new ArgumentNullException(nameof(modelName));
            ModelType = null;
        }

        /// <summary>
        /// Возвращает тип модели
        /// </summary>     
        public Type ModelType { get; }

        /// <summary>
        /// Возвращает путь к модели
        /// </summary>  
        public string ModelPath { get; }

        /// <summary>
        /// Возвращает источник <see cref="BindingSource.DataContext"/>
        /// </summary>
        public BindingSource BindingSource => BindingSource.DataContext;
    }
}
