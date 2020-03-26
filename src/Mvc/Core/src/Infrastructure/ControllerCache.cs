using EquipApps.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Кеш контроллеров. 
    /// </summary>
    public class ControllerCache
    {
        private readonly Func<Type, ObjectFactory> _createFactory = (type) => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);
        private readonly ConcurrentDictionary<Type, ObjectFactory>  _typeActivatorCache = new ConcurrentDictionary<Type, ObjectFactory>();
        private readonly ConcurrentDictionary<Type, object>         _typeEntriesCache   = new ConcurrentDictionary<Type, object>();
        private readonly IServiceProvider _serviceProvider;

        private WeakReference<ControllerTestCase> weak_lastTestCase = new WeakReference<ControllerTestCase>(null);

        public ControllerCache(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetCachedResult(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }

            /*
             * ВРЕМЯ ЖИЗНИ ВСЕХ КОНТРОЛЛЕРОВ ОГРАНИЧИВАЕТСЯ ТЕСТОВЫМ СЛУЧАЕМ!
             */
            var rootTestCase = controllerContext.ActionDescriptor.TestCase;

            //-- Запрашиваем последний тестовый случай.
            if (weak_lastTestCase.TryGetTarget(out var lastTestCase))
            {
                if (rootTestCase != lastTestCase)
                {
                    //-- Если последний не совпадает с текущим, то очищаем кеш
                    _typeEntriesCache.Clear();
                    weak_lastTestCase.SetTarget(rootTestCase);
                }
            }
            else
                weak_lastTestCase.SetTarget(rootTestCase);


            return _typeEntriesCache.GetOrAdd(rootTestCase.ControllerModel.Info, CreateController);
        }

        private object CreateController(Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            var createFactory = _typeActivatorCache.GetOrAdd(controllerType, _createFactory);

            return createFactory(_serviceProvider, arguments: null);
        }
    }
}
