using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.ApplicationParts
{
    /// <summary>
    /// Singleton. Менеджер приложения
    /// </summary>
    public class ApplicationPartManager
    {
        /// <summary>
        /// Возвращает коллекцию <see cref="IApplicationFeatureProvider"/>
        /// </summary>
        public IList<IApplicationFeatureProvider> FeatureProviders { get; } =
            new List<IApplicationFeatureProvider>();

        /// <summary>
        /// Возвращает коллекцию <see cref="ApplicationPart"/>
        /// </summary>
        public IList<ApplicationPart> ApplicationParts { get; } =
           new List<ApplicationPart>();

        /// <summary>
        /// Конфигурирует функцию приложения
        /// </summary>
        /// 
        /// <typeparam name="TFeature">
        /// Тип расширения
        /// </typeparam>
        /// 
        /// <param name="feature">
        /// Расширение
        /// </param>
        public void PopulateFeature<TFeature>(TFeature feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            foreach (var provider in FeatureProviders.OfType<IApplicationFeatureProvider<TFeature>>())
            {
                provider.PopulateFeature(ApplicationParts, feature);
            }
        }
    }
}
