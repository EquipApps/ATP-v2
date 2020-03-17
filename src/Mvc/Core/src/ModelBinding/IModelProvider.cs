using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Инфроструктурa провайдера моделей
    /// </summary>
    public interface IModelProvider
    {
        //TODO: Переделать в IQuerable

        /// <summary>
        /// Возвращает коллекцию моделей
        /// </summary>      
        IReadOnlyList<object> Provide();
    }
}
