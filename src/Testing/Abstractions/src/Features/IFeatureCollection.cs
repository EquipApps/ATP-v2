using System;
using System.Collections.Generic;

namespace EquipApps.Testing.Features
{
    /// <summary>
    /// Предоставляет коллекцию расширений
    /// </summary>
    public interface IFeatureCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        /// <summary>
        ///  Задает или возвращает фичу. Установка null значения удаляет фичу
        /// </summary>      
        object this[Type key] { get; set; }

        /// <summary>
        /// Возвращает <typeparamref name="TFeature"/> фичу
        /// </summary>
        /// 
        /// <typeparam name="TFeature">
        /// Тип фичи
        /// </typeparam>       
        TFeature Get<TFeature>();

        /// <summary>
        /// Задает <typeparamref name="TFeature"/> фичу
        /// </summary>
        /// 
        /// <typeparam name="TFeature">
        /// Тип фичи
        /// </typeparam>       
        void Set<TFeature>(TFeature instance);
    }
}
