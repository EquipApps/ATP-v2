using System;

namespace EquipApps.Testing.Features
{
    /// <summary>
    /// Контекст провайдера расширений.
    /// </summary>
    public class FeatureProviderContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        /// <param name="collection">
        /// Пустая коллекция расширений
        /// </param>
        public FeatureProviderContext(IFeatureCollection collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>
        /// Возвращает <see cref="IFeatureCollection"/>
        /// </summary>
        public IFeatureCollection Collection { get; }
    }
}
