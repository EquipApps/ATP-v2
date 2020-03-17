using System.Collections.Generic;

namespace EquipApps.Mvc.ApplicationParts
{
    /// <summary>
    /// Провайдер <typeparamref name="TFeature"/> функции приложения
    /// </summary>
    public interface IApplicationFeatureProvider<TFeature> : IApplicationFeatureProvider
    {
        /// <summary>
        /// Конфигурирует функцию приложения
        /// </summary>
        /// 
        /// <param name="parts">
        /// Все составные части приложения
        /// </param>
        /// 
        /// <param name="feature">
        /// Функция приложения
        /// </param>
        void PopulateFeature(IEnumerable<ApplicationPart> parts, TFeature feature);
    }
}
