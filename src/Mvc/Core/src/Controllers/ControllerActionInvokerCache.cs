using EquipApps.Mvc.ApplicationModels;
using System;
using System.Collections.Concurrent;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    [Obsolete("Переделать ключ(на тип!)!")]
    public class ControllerActionInvokerCache
    {
        //---
        private ConcurrentDictionary<MethodModel, ControllerActionInvokerCacheEntry> Entries
            = new ConcurrentDictionary<MethodModel, ControllerActionInvokerCacheEntry>();

        public ControllerActionInvokerCacheEntry GetCachedResult(ControllerContext controllerContext)
        {
            try
            {
                //-- Получаем модель действия!
                var actionModel = controllerContext
                    .ActionDescriptor
                    .TestStep
                    .ActionModel;

                if (!Entries.TryGetValue(actionModel, out var cacheEntry))
                {
                    var methodInfo = actionModel.Info;
                    var targetInfo = actionModel.Controller.Info;

                    //-- Создаем параметры по умолчаению!
                    var parameterDefaultValues = ParameterDefaultValues.GetParameterDefaultValues(methodInfo);

                    var objectMethodExecutor = ObjectMethodExecutor.Create(methodInfo, targetInfo, parameterDefaultValues);
                    var сontrollerMethodExecutor = ControllerMethodExecutor.GetExecutor(objectMethodExecutor);

                    cacheEntry = new ControllerActionInvokerCacheEntry(
                        objectMethodExecutor,
                        сontrollerMethodExecutor);

                    cacheEntry = Entries.GetOrAdd(actionModel, cacheEntry);
                }

                return cacheEntry;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
