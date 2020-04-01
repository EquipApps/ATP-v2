using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            //-- 
            var objectMethodExecutor = controllerActionCacheEntry.ObjectMethodExecutor;
            var actionMethodExecutor = controllerActionCacheEntry.ActionMethodExecutor;

            //-- Создаем контроллер
            var controller = controlleFactoryDelegate(controllerContext);

            var actionParameters = new Dictionary<string, object>();

            BindProperty(controller);                                                   //-- Привязка свойств            
            BindParameters(actionParameters);                                             //-- Привзяка параметров
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
            var properties = controllerContext
                .ActionDescriptor
                .TestCase
                .ControllerModel
                .ControllerProperties;

            var framworkElement = controllerContext
               .ActionDescriptor
               .TestCase;

            foreach (var property in properties)
            {
                //-- У свойства нету привязки! (Возможно ошибка модели приложения)
                if (property.ModelBinder == null)
                {
                    logger.LogWarning(
                        $"У свойства нет привязки. (Возможно ошибка модели приложения) " +
                        $"Область: {property.Controller.Area}; " +
                        $"Контроллер: {property.Controller.ControllerName}; " +
                        $"Свойство: {property.PropertyName};");
                    continue;
                }

                var resultBinding = property.ModelBinder.Bind(framworkElement, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Область: {property.Controller.Area}; " +
                        $"Контроллер: {property.Controller.ControllerName}; " +
                        $"Свойство: {property.PropertyName};");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Область: {property.Controller.Area}; " +
                    $"Контроллер: {property.Controller.ControllerName}; " +
                    $"Свойство: {property.PropertyName};");
                }

                try
                {
                    property.PropertyInfo.SetValue(controller, resultBinding.Model);
                }
                catch (Exception ex)
                {
                    //TODO: Добавить информацию об ошибке модели!

                    logger.LogError(ex,
                    $"Не получилось присвоить значение свойству. " +
                    $"Область: {property.Controller.Area}; " +
                    $"Контроллер: {property.Controller.ControllerName}; " +
                    $"Свойство: {property.PropertyName};");

                    throw ex;
                }
            }
        }
        private void BindParameters(Dictionary<string, object> arguments)
        {
            var framworkElement = controllerContext
                .ActionDescriptor
                .TestStep;

            var bindingInfo = controllerContext
                .ActionDescriptor
                .TestStep
                .ActionModel
                .BindingInfo;

            var parameters = controllerContext
                .ActionDescriptor
                .TestStep
                .ActionModel
                .Parameters;

            foreach (var parameter in parameters)
            {
                //-- У параметра нету привязки! (Возможно ошибка модели приложения)
                if (parameter.ModelBinder == null)
                {
                    logger.LogWarning(
                        $"У свойства нет привязки. (Возможно ошибка модели приложения) " +
                        $"Область: {parameter.Action.Controller.Area}; " +
                        $"Контроллер: {parameter.Action.Controller.ControllerName}; " +
                        $"Метод: {parameter.Action.ActionName}" +
                        $"Аргумент: {parameter.ParameterName};");

                    continue;
                }

                var resultBinding = parameter.ModelBinder.Bind(framworkElement, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Область: {parameter.Action.Controller.Area}; " +
                        $"Контроллер: {parameter.Action.Controller.ControllerName}; " +
                        $"Метод: {parameter.Action.ActionName}" +
                        $"Аргумент: {parameter.ParameterName};");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Область: {parameter.Action.Controller.Area}; " +
                        $"Контроллер: {parameter.Action.Controller.ControllerName}; " +
                        $"Метод: {parameter.Action.ActionName}" +
                        $"Аргумент: {parameter.ParameterName};");

                }

                arguments[parameter.ParameterName] = resultBinding.Model;
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
