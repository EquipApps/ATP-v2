using EquipApps.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    [Obsolete("Реализовать валидацию модели и сохранение Состояние в controller context")]
    public class ControllerActionInvoker : IActionInvoker
    {
        private ILogger<ControllerActionInvoker> logger;
        private ControllerContext controllerContext;
        private ControllerActionInvokerCacheEntry controllerActionCacheEntry;
        private ControlleFactoryDelegate controlleFactoryDelegate;

        public ControllerActionInvoker(
            ControllerContext controllerContext,
            ControllerActionInvokerCacheEntry controllerActionCacheEntry,
            ControlleFactoryDelegate controlleFactoryDelegate,
            ILogger<ControllerActionInvoker> logger
            )
        {
            this.controllerContext          = controllerContext ?? throw new ArgumentNullException(nameof(controllerContext));
            this.controllerActionCacheEntry = controllerActionCacheEntry ?? throw new ArgumentNullException(nameof(controllerActionCacheEntry));
            this.controlleFactoryDelegate   = controlleFactoryDelegate ?? throw new ArgumentNullException(nameof(controlleFactoryDelegate));
            this.logger                     = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Invoke()
        {
            //-- 
            var objectMethodExecutor = controllerActionCacheEntry.ObjectMethodExecutor;
            var actionMethodExecutor = controllerActionCacheEntry.ActionMethodExecutor;

            //-- Создаем контроллер
            var controller       = controlleFactoryDelegate(controllerContext);
            var actionParameters = new Dictionary<string, object>();

            //-- Привязка
            BindProperty    (controller);
            BindParameters  (actionParameters);

            //-- BindContext
            var controllerBase = controller as ControllerBase;
            if (controllerBase != null)
            {
                controllerBase.ControllerContext = controllerContext;
                controllerBase.InitializeComponent();
            }

            

            //-- Извлечение аргуменов функции
            var arguments = PrepareArguments(actionParameters, objectMethodExecutor);


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



        





        private void BindProperty(object controller)
        {
            var properties = controllerContext
                .ActionDescriptor
                .TestCase
                .ControllerModel
                .Properties;

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
                        $"Контроллер: {property.Controller.Name}; " +
                        $"Свойство: {property.Name};");
                    continue;
                }

                var  resultBinding = property.ModelBinder.Bind(framworkElement, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Область: {property.Controller.Area}; " +
                        $"Контроллер: {property.Controller.Name}; " +
                        $"Свойство: {property.Name};");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Область: {property.Controller.Area}; " +
                    $"Контроллер: {property.Controller.Name}; " +
                    $"Свойство: {property.Name};");
                }

                try
                {
                    property.Info.SetValue(controller, resultBinding.Model);
                }
                catch (Exception ex)
                {
                    //TODO: Добавить информацию об ошибке модели!

                    logger.LogError(ex,
                    $"Не получилось присвоить значение свойству. " +
                    $"Область: {property.Controller.Area}; " +
                    $"Контроллер: {property.Controller.Name}; " +
                    $"Свойство: {property.Name};");

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
                        $"Область: {parameter.Method.Controller.Area}; " +
                        $"Контроллер: {parameter.Method.Controller.Name}; " +
                        $"Метод: {parameter.Method.Name}" +
                        $"Аргумент: {parameter.Name};");

                    continue;
                }

                var resultBinding = parameter.ModelBinder.Bind(framworkElement, 1);
                if(!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        $"Не получилось привязаться к данным " +
                        $"Область: {parameter.Method.Controller.Area}; " +
                        $"Контроллер: {parameter.Method.Controller.Name}; " +
                        $"Метод: {parameter.Method.Name}" +
                        $"Аргумент: {parameter.Name};");

                    //TODO: Добавить информацию об ошибке модели!

                    continue;
                }

                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Область: {parameter.Method.Controller.Area}; " +
                        $"Контроллер: {parameter.Method.Controller.Name}; " +
                        $"Метод: {parameter.Method.Name}" +
                        $"Аргумент: {parameter.Name};");

                }


                arguments[parameter.Name] = resultBinding.Model;
            }
        }
       


        private static object[] PrepareArguments(Dictionary<string, object> actionParameters, ObjectMethodExecutor actionMethodExecutor)
        {
            var declaredParameterInfos = actionMethodExecutor.MethodParameters;
            var count = declaredParameterInfos.Length;
            if (count == 0)
            {
                return null;
            }

            var arguments = new object[count];
            for (var index = 0; index < count; index++)
            {
                var parameterInfo = declaredParameterInfos[index];

                if (!actionParameters.TryGetValue(parameterInfo.Name, out var value))
                {
                    value = actionMethodExecutor.GetDefaultValueForParameter(index);
                }

                arguments[index] = value;
            }

            return arguments;
        }
    }
}
