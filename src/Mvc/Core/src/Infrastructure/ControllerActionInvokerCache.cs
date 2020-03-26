using EquipApps.Mvc.ApplicationModels;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Concurrent;

namespace EquipApps.Mvc.Infrastructure
{    
    public class ControllerActionInvokerCache
    {
        private volatile InnerCache _currentCache;
        private IActionDescriptorCollectionProvider _collectionProvider;

        private InnerCache CurrentCache
        {
            get
            {
                var current = _currentCache;
                var actionDescriptors = _collectionProvider.ActionDescriptors;

                if (current == null || current.Version != actionDescriptors.Version)
                {
                    current = new InnerCache(actionDescriptors.Version);
                    _currentCache = current;
                }

                return current;
            }
        }

        public ControllerActionInvokerCache(
            IActionDescriptorCollectionProvider collectionProvider)
        {
            _collectionProvider = collectionProvider;
        }  

        public ControllerActionInvokerCacheEntry GetCachedResult(ControllerContext controllerContext)
        {
            try
            {
                var cache = CurrentCache;
                var actionDescriptor = controllerContext.ActionDescriptor;

                //TODO: Ключь сделать ActionDescriptor?
                //-- Получаем модель действия!
                var actionModel = controllerContext
                    .ActionDescriptor
                    .TestStep
                    .ActionModel;

                if (!cache.Entries.TryGetValue(actionModel, out var cacheEntry))
                {
                    var methodInfo = actionModel.Info;
                    var targetInfo = actionModel.Controller.Info;

                    //-- Создаем параметры по умолчаению!
                    var parameterDefaultValues = ParameterDefaultValues.GetParameterDefaultValues(methodInfo);

                    var objectMethodExecutor = ObjectMethodExecutor.Create(methodInfo, targetInfo, parameterDefaultValues);
                    var actionMethodExecutor = ActionMethodExecutor.GetExecutor(objectMethodExecutor);

                    cacheEntry = new ControllerActionInvokerCacheEntry(
                        objectMethodExecutor,
                        actionMethodExecutor);

                    cacheEntry = cache.Entries.GetOrAdd(actionModel, cacheEntry);
                }

                return cacheEntry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class InnerCache
        {
            public InnerCache(int version)
            {
                Version = version;
            }

            public ConcurrentDictionary<MethodModel, ControllerActionInvokerCacheEntry> Entries { get; } =
                new ConcurrentDictionary<MethodModel, ControllerActionInvokerCacheEntry>();

            public int Version { get; }
        }
    }
}
