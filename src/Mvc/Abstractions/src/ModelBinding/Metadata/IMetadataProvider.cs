using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.ModelBinding.Metadata
{
    /// <summary>
    /// Провайдер <see cref="ModelMetadata"/>
    /// </summary>
    public interface IMetadataProvider
    {
        /// <summary>
        /// Создает <see cref="ModelMetadata"/> для конкретного <see cref="Type"/>
        /// </summary>
        /// 
        /// <param name="modelType">
        /// Тип для которого создаются методанные
        /// </param>       
        ModelMetadata GetMetadataForType(Type modelType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType);
    }
}
