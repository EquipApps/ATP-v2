using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктурa провайдера моделей
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// Тип модели
    /// </typeparam> 
    public interface IModelProvider<T> : IModelProvider where T : class
    {
        /// <summary>
        /// Возвращает коллекцию моделей
        /// </summary>   
        new IReadOnlyList<T> Provide();
    }
}
