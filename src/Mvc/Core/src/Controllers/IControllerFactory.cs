using System;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    /// <remarks>
    /// Используется для кастоизации создания контроллера.  
    /// </remarks>
    public interface IControllerFactory
    {
        /// <summary>
        /// Создает новый контроллер
        /// </summary> 
        object CreateController(Type descriptor);

        /// <summary>
        /// Освобаждает ресурсы.
        /// </summary>  
        void ReleaseController(object controller);
    }
}
