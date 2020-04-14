using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.Infrastructure
{
    [Obsolete("Реализовать валидацию модели и сохранение Состояние в controller context")]
    public class ControllerActionInvoker : IActionInvoker
    {
        private ILogger logger;
        private ControllerContext controllerContext;
        private ControllerActionInvokerCacheEntry controllerActionCacheEntry;
        private ControlleFactoryDelegate controlleFactoryDelegate;

        public ControllerActionInvoker(ControllerContext controllerContext,
                                       ControllerActionInvokerCacheEntry controllerActionCacheEntry,
                                       ControlleFactoryDelegate controlleFactoryDelegate,
                                       ILogger logger)
        {
            this.controllerContext = controllerContext ?? throw new ArgumentNullException(nameof(controllerContext));
            this.controllerActionCacheEntry = controllerActionCacheEntry ?? throw new ArgumentNullException(nameof(controllerActionCacheEntry));
            this.controlleFactoryDelegate = controlleFactoryDelegate ?? throw new ArgumentNullException(nameof(controlleFactoryDelegate));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Invoke()
        {
            try
            {
                InvokeInternel();
            }
            catch (Exception ex)
            {
                var exception = ex;
                //TODO: Написать юнит тесты для поиска состояния остановка!
                if (exception is TargetInvocationException)
                {
                    exception = ex.InnerException ?? ex;
                }
                if (exception is AggregateException)
                {
                    exception = ex.InnerException ?? ex;
                }

                if (exception is OperationCanceledException)
                {
                    controllerContext.ActionDescriptor.Result = Abstractions.Result.NotRun;
                    controllerContext.ActionDescriptor.Exception = exception;
                }
                else
                {
                    controllerContext.ActionDescriptor.Result = Abstractions.Result.Failed;
                    controllerContext.ActionDescriptor.Exception = exception;
                }
            }
        }

        private void InvokeInternel()
        {
            //-- 
            var objectMethodExecutor = controllerActionCacheEntry.ObjectMethodExecutor;
            var actionMethodExecutor = controllerActionCacheEntry.ActionMethodExecutor;

            //-- Создаем контроллер
            var controller = controlleFactoryDelegate(controllerContext);

            var actionParameters = new Dictionary<string, object>();

            BindProperty(controller);                                                       //-- Привязка свойств            
            BindParameters(actionParameters);                                               //-- Привзяка параметров
            PrepareArguments(actionParameters, objectMethodExecutor, out var arguments);    //-- Извлечение аргуменов функции

            //-- Set Context
            var controllerBase = controller as ControllerBase;
            if (controllerBase != null)
            {
                controllerBase.ControllerContext = controllerContext;
                controllerBase.InitializeComponent();
            }

            /*
             * Оборачиваем все сообщения внутри вызова.
             * Необходимо для поиска сообщений
             */
            using (var scope = controllerContext.TestContext.TestLogger.BeginScope(controllerContext.ActionDescriptor.Number))
            {
                //-- Выполняем Действие   
                var result = actionMethodExecutor.Execute2(objectMethodExecutor, controller, arguments);

                //-- Выполняем Результат        
                if (result != null)
                {
                    result.ExecuteResult(controllerContext);
                }

                //-- Выполняем завершающую часть
                if (controllerBase != null)
                {
                    controllerBase.Finally();
                }
            }
        }



        private void BindProperty(object controller)
        {
            var properties = controllerContext.ActionDescriptor.BoundBinderItem;
            if (properties == null)
            {
                return;
            }

            var framworkElement = controllerContext.ActionDescriptor.TestCase;
            
            foreach (var property in properties)
            {
                //-- У свойства нету привязки! (Возможно ошибка модели приложения)
                if (property.ModelBinder == null)
                {
                    logger.LogWarning(
                        $"У свойства нет привязки. (Возможно ошибка модели приложения) " +
                        $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                        $"Свойство:   {property.ModelMetadata.Name};");
                    continue;
                }

                var result = property.ModelBinder.Bind(framworkElement, 1);
                if (!result.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                        $"Свойство:   {property.ModelMetadata.Name};");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (result.IsModelNull)
                {
                    logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                    $"Свойство:   {property.ModelMetadata.Name};");
                }

                try
                {
                    PropertyValueSetter.SetValue(property.ModelMetadata, controller, result.Model);
                }
                catch (Exception ex)
                {
                    //TODO: Добавить информацию об ошибке модели!

                    logger.LogError(ex,
                    $"Не получилось присвоить значение свойству. " +
                    $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                    $"Свойство:   {property.ModelMetadata.Name};");

                    throw ex;
                }
            }
        }
        private void BindParameters(Dictionary<string, object> arguments)
        {
            var parameters = controllerContext.ActionDescriptor.BinderItem;
            if (parameters == null)
            {
                return;
            }

            var framworkElement = controllerContext.ActionDescriptor.TestStep;


            foreach (var parameter in parameters)
            {
                //-- У параметра нету привязки! (Возможно ошибка модели приложения)
                if (parameter.ModelBinder == null)
                {
                    logger.LogWarning(
                        $"У свойства нет привязки. (Возможно ошибка модели приложения) " +
                        $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                        $"Метод: {controllerContext.ActionDescriptor.ActionName}; " +
                        $"Аргумент: {parameter.ModelMetadata.Name}");
                      

                    continue;
                }

                var resultBinding = parameter.ModelBinder.Bind(framworkElement, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                        $"Метод: {controllerContext.ActionDescriptor.ActionName}; " +
                        $"Аргумент: {parameter.ModelMetadata.Name}");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Контроллер: {controllerContext.ActionDescriptor.ControllerName}; " +
                        $"Метод: {controllerContext.ActionDescriptor.ActionName}; " +
                        $"Аргумент: {parameter.ModelMetadata.Name}");

                }

                arguments[parameter.ModelMetadata.Name] = resultBinding.Model;
            }
        }
        private void PrepareArguments(Dictionary<string, object> actionParameters, ObjectMethodExecutor objectMethodExecutor, out object[] arguments)
        {
            var declaredParameterInfos = objectMethodExecutor.MethodParameters;

            var count = declaredParameterInfos.Length;
            if (count == 0)
            {
                arguments = null;
                return;
            }

            arguments = new object[count];
            for (var index = 0; index < count; index++)
            {
                var parameterInfo = declaredParameterInfos[index];

                if (!actionParameters.TryGetValue(parameterInfo.Name, out var value))
                {
                    value = objectMethodExecutor.GetDefaultValueForParameter(index);
                }

                arguments[index] = value;
            }
        }
    }
}
