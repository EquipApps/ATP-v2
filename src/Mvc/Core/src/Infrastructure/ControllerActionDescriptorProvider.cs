using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.Internal
{
    public class ControllerActionDescriptorProvider : IActionDescriptorProvider
    {
        private ApplicationModelFactory appModelFactory;
        private ILogger<ControllerActionDescriptorProvider> logger;
        private MvcOptions options;

        private int startIndex = 0;

        public ControllerActionDescriptorProvider(ApplicationModelFactory appModelFactory,
                                                  ILoggerFactory loggerFactory,
                                                  IOptions<MvcOptions> options)
        {
            this.appModelFactory = appModelFactory ?? throw new ArgumentNullException(nameof(appModelFactory));
            this.options         = options?.Value  ?? throw new ArgumentNullException(nameof(options));

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<ControllerActionDescriptorProvider>();

            startIndex = this.options.StartIndex;
        }

        public int Order => -1000;

        public void OnProvidersExecuting(ActionDescriptorProviderContext context)
        {
            //-- Создаем модель приложения.
            var application = appModelFactory.GetApplicationModel();

            if (application == null)
            {
                logger.LogError("Не удалось создать модель приложения!");
                throw new NullReferenceException(nameof(application));
            }

            //-- Проверка.
            if (application.Controllers.Count == 0)
            {
                logger.LogError("Модель приложения не содержит контроллеров!");
            }

            //-- Создаем тестовые наборы
            var result = application.Controllers
                .GroupBy(x => x.Area)
                .OrderBy(x => x.Key)                //TODO: Cортировака по ApplicationModel.Areas.Index
                .SelectMany(ToTestSuitOrderByNumber)
                .SelectMany(ToActionDescriptor)

                .OrderBy(x => x.Number)
                .OrderBy(x => x.Area);


            context.Results.AddRange(result);
        }

        public void OnProvidersExecuted(ActionDescriptorProviderContext context)
        {
            //-- Ничего не делаем!
        }

        //TODO: Юнит тест проверка порядка!
        private IEnumerable<ControllerTestCase> ToTestSuitOrderByNumber(IEnumerable<ControllerModel> controllerModels)
        {
            var controllers = new SortedList<int, ControllerModel>();

            var controllerHasNumber = controllerModels
                .Where(x => x.Index.HasValue)
                .ToArray();

            var controllerNulNumber = controllerModels
                .Where(x => !x.Index.HasValue)
                .ToArray();

            //-- 1) Сначало записвываем в список контроллеры с номерами

            foreach (var controller in controllerHasNumber)
            {
                if (controllers.ContainsKey(controller.Index.Value))
                {
                    logger.LogError(
                       $"Ошибка. В одной области не могут содержать контроллеры с одинаковыми индексами! " +
                       $"Область: {controller.Area}; Контроллер: {controller.ControllerName}; Индекс: {controller.Index}; " +
                       $"будет пропущен пропущен");
                }
                else
                    controllers.Add(controller.Index.Value, controller);
            }


            //-- 2) Затем контроллеры без номеров вставляем в список
            for (int i = 0, index = startIndex; i < controllerNulNumber.Length; i++, index++)
            {
                var controller = controllerNulNumber[i];

                //-- Увеличивем счетчик пока не найдем свободный
                while (controllers.ContainsKey(index))
                    index++;

                controllers.Add(index, controller);
            }

            //-- 3) Преобразовываем

            foreach (var controllerPair in controllers)
            {
                var controllerModelIndex = controllerPair.Key;
                var controllerModel = controllerPair.Value;
                var modelBinder = controllerModel.ModelBinder;

                //-- 1) Нету привязки? создаем одиночный TestCase
                if (modelBinder == null)
                {
                    yield return new ControllerTestCase(controllerModel, controllerModelIndex);
                    continue;
                }

                //-- 2) Не получилось привязаться к данным? пропускаем.
                var resultBinding = modelBinder.Bind(null, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        resultBinding.Exception,
                        $"Не получилось привязаться к данным. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName};");
                    continue;
                }

                //-- 3) привязка вернула null данным? пропускаем.
                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName};");
                    continue;
                }

                //-- 4) Количество данных равно единице? возвращаем один TestCase
                if (!resultBinding.IsMany)
                {
                    // TODO: Проверить в юнит тесте. Background всегда ли отлична от нуля когда есть привязка?
                    // TODO: Если один тест кейс, может не использовать индекс?
                    var testCase = new ControllerTestCase(controllerModel, controllerModelIndex);
                    testCase.DataContext = resultBinding.Model;

                    yield return testCase;
                    continue;
                }

                //-- 5) Массив
                var bindingResults = resultBinding.GetResults();
                if (bindingResults.Length == 0)
                    logger.LogWarning(
                        $"Привязка к пустой коллекции. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName};");

                for (int i = 0, index = startIndex; i < bindingResults.Length; i++, index++)
                {
                    /*  
                     *  Если результат не удалось получить, то пропускаем данный пункт проверки! (НО ИНДЕКС УВЕЛИЧИВАЕТСЯ)
                     *  Нужно для маскирования ПУСТЫХ СТРОК в базе данных!
                     */
                    var bindingResult = bindingResults[i];

                    if (!bindingResult.IsModelSet)
                    {
                        logger.LogError(
                        resultBinding.Exception,
                        $"Не получилось привязаться к данным. " +
                        $"Index:{i}; Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName};");

                        continue;
                    }

                    if (bindingResult.IsModelNull)
                    {
                        logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Index:{i}; Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName};");

                        continue;
                    }

                    var testCase = new ControllerTestCase(controllerModel, controllerModelIndex, index);
                    testCase.DataContext = bindingResult.Model;

                    yield return testCase;
                }
            }
        }

        private IEnumerable<ControllerActionDescriptor> ToActionDescriptor(ControllerTestCase testCase)
        {
            foreach (var testStep in ToTestSteps(testCase))
            {
                testStep.Parent = testCase;
                testCase.TestSteps.Add(testStep);

                yield return new ControllerActionDescriptor(testCase, testStep);
            }
        }

        private IEnumerable<ControllerTestStep> ToTestSteps(ControllerTestCase testCase)
        {
            var controllerModel = testCase.ControllerModel;
            var methods = controllerModel.Actions;

            for (int actionModelIndex = 0; actionModelIndex < methods.Count; actionModelIndex++)
            {
                var method = methods[actionModelIndex];
                var methodModelBinder = method.ModelBinder;

                //-- 1) Нету привязки? создаем одиночный TestStep
                if (methodModelBinder == null)
                {
                    yield return new ControllerTestStep(method, actionModelIndex);
                    continue;
                }

                //-- 2) Не получилось привязаться к данным? пропускаем.
                var resultBinding = methodModelBinder.Bind(testCase, 1);
                if (!resultBinding.IsModelSet)
                {
                    logger.LogError(
                        resultBinding.Exception,
                        $"Не получилось привязаться к данным. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName}; Метод: {method.ActionName};");

                    continue;
                }

                //-- 3) привязка вернула null данным? пропускаем.
                if (resultBinding.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName}; ; Метод: {method.ActionName};");

                    continue;
                }

                //-- 4) Количество данных равно единице? возвращаем один TestStep
                if (!resultBinding.IsMany)
                {
                    var testStep = new ControllerTestStep(method, actionModelIndex);
                    testStep.DataContext = resultBinding.Model;
                    yield return testStep;
                    continue;
                }

                //-- 5) Массив
                var bindingResults = resultBinding.GetResults();
                if (bindingResults.Length == 0)
                    logger.LogWarning(
                        $"Привязка к пустой коллекции. " +
                        $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName}; Метод: {method.ActionName};");


                for (int i = 0; i < bindingResults.Length; i++)
                {
                    var bindingResult = bindingResults[i];

                    /*
                     * Если результат неудалось получить, то пропускаем данный пункт проверки! (НО ИНДЕКС УВЕЛИЧИВАЕТСЯ)
                     * Нужно для маскирования ПУСТЫХ СТРОК в базе данных!
                     */
                    if (!bindingResult.IsModelSet)
                    {
                        logger.LogError(
                            bindingResult.Exception,
                            $"Не получилось привязаться к данным. Index:{i}; " +
                            $"Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName}; Метод: {method.ActionName};");

                        continue;
                    }

                    if (bindingResult.IsModelNull)
                    {
                        logger.LogWarning(
                            $"Модель привязки NULL. " +
                            $"Index:{i}; Область: {controllerModel.Area}; Контроллер: {controllerModel.ControllerName}; Метод: {method.ActionName};");

                        continue;
                    }


                    var testStep = new ControllerTestStep(method, actionModelIndex, i);
                    testStep.DataContext = bindingResult.Model;

                    yield return testStep;
                }
            }
        }

    }
}
