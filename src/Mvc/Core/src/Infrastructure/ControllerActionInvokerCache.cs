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

        

        public ControllerActionInvokerCache(
            IActionDescriptorCollectionProvider collectionProvider)
        {
            _collectionProvider = collectionProvider;
        }

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

        public ControllerActionInvokerCacheEntry GetCachedResult(ControllerContext controllerContext)
        {
            try
            {
                var cache = CurrentCache;
                var actionDescriptor = controllerContext.ActionDescriptor;

                if (!cache.Entries.TryGetValue(actionDescriptor, out var cacheEntry))
                {
                    //-- Создаем параметры по умолчаению!
                    var parameterDefaultValues = ParameterDefaultValues
                        .GetParameterDefaultValues(actionDescriptor.MethodInfo);

                    var objectMethodExecutor = ObjectMethodExecutor.Create(                        
                        actionDescriptor.MethodInfo,
                        actionDescriptor.ControllerTypeInfo,
                        parameterDefaultValues);

                    var actionMethodExecutor = ActionMethodExecutor.GetExecutor(objectMethodExecutor);

                    cacheEntry = new ControllerActionInvokerCacheEntry(
                        objectMethodExecutor,
                        actionMethodExecutor);

                    cacheEntry = cache.Entries.GetOrAdd(actionDescriptor, cacheEntry);
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

            public ConcurrentDictionary<ActionDescriptor, ControllerActionInvokerCacheEntry> Entries { get; } =
                new ConcurrentDictionary<ActionDescriptor, ControllerActionInvokerCacheEntry>();

            public int Version { get; }
        }
    }
}
