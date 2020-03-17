using System;
using System.Collections.Concurrent;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    /// <summary>
    /// Кеш контроллеров. Обеспечивает время жизни контроллера 
    /// </summary>
    public class ControllerCache
    {
        private WeakReference<ControllerTestCase> weak_lastTestCase = new WeakReference<ControllerTestCase>(null);

        private readonly ConcurrentDictionary<Type, object> cacheEntries = new ConcurrentDictionary<Type, object>();
        private readonly IControllerFactory _controllerFactory;

        public ControllerCache(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory ?? throw new ArgumentNullException(nameof(controllerFactory));
        }

        public object GetCachedResult(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }

            var rootTestCase = controllerContext
                .ActionDescriptor
                .TestCase;

           

            /*
             * ВРЕМЯ ЖИЗНИ ВСЕХ КОНТРОЛЛЕРОВ ОГРАНИЧИВАЕТСЯ ТЕСТОВМ СЛУЧАЕМ!
             */

            //-- Запрашиваем последний тестовый случай.
            if (weak_lastTestCase.TryGetTarget(out var lastTestCase))
            {
                if (rootTestCase != lastTestCase)
                {
                    //-- Если последний не совпадает с текущим, то очищаем кеш
                    cacheEntries.Clear();
                    weak_lastTestCase.SetTarget(rootTestCase);
                }
            }
            else
                weak_lastTestCase.SetTarget(rootTestCase);


            return cacheEntries.GetOrAdd(rootTestCase.ControllerModel.Info, _controllerFactory.CreateController);
        }


        public void Clear()
        {
            cacheEntries.Clear();
        }
    }
}
