using System;

namespace EquipApps.Mvc.ModelBinding.Property
{
    public class PropertyEntery
    {
        public PropertyEntery(PropertyExtractor extractor, Type modelType)
        {
            Extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
            Type = modelType ?? throw new ArgumentNullException(nameof(modelType)); ;
        }

        /// <summary>
        /// Возвращает <see cref="PropertyExtractor"/>
        /// </summary>
        public PropertyExtractor Extractor { get; }

        /// <summary>
        /// Возвращает <see cref="System.Type"/> свойтва.
        /// </summary>
        public Type Type { get; }
    }
}
