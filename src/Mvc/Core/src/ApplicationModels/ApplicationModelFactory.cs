using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Controllers;
using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class ApplicationModelFactory
    {
        private readonly IApplicationModelProvider[] _applicationModelProviders;
        private readonly IList<IApplicationModelConvention> _conventions;
        private readonly IModelBindingFactory _modelBinderFactory;
        private ILogger<ApplicationModelFactory> logger;
        private readonly int _startIndex = 0;

        public ApplicationModelFactory(
            IEnumerable<IApplicationModelProvider> applicationModelProviders,
            IOptions<MvcOptions> options,
            IModelBindingFactory modelBinderFactory,
            ILoggerFactory loggerFactory)
        {
            if (applicationModelProviders == null)
            {
                throw new ArgumentNullException(nameof(applicationModelProviders));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _modelBinderFactory = modelBinderFactory ?? throw new ArgumentNullException(nameof(modelBinderFactory));

            logger = loggerFactory.CreateLogger<ApplicationModelFactory>();

            _applicationModelProviders = applicationModelProviders.OrderBy(p => p.Order).ToArray();
            _conventions = options.Value.Conventions;
            _startIndex = options.Value.StartIndex;
        }

        public ApplicationModel CreateApplicationModel(IEnumerable<TypeInfo> controllerTypes)
        {
            if (controllerTypes == null)
            {
                throw new ArgumentNullException(nameof(controllerTypes));
            }

            var context = new ApplicationModelProviderContext(controllerTypes);

            for (var i = 0; i < _applicationModelProviders.Length; i++)
            {
                _applicationModelProviders[i].OnProvidersExecuting(context);
            }

            for (var i = _applicationModelProviders.Length - 1; i >= 0; i--)
            {
                _applicationModelProviders[i].OnProvidersExecuted(context);
            }

            ApplicationModelConventions.ApplyConventions(context.Result, _conventions);

            return context.Result;
        }

        public IList<ControllerActionDescriptor> Builder(ApplicationModel application)
        {
            var results = new List<ControllerActionDescriptor>();

            foreach (var controller in application.Controllers)
            {
                //-- Создаем Тестовый набор для данного контроллера
                var controllerTestCases = GetTestCases(controller).ToArray();

                //-- Заполняем
                FillDisplayInfo(controller, controllerTestCases);

                foreach (var action in controller.Actions)
                {
                    var actionTitleBinder  = _modelBinderFactory.Create(action, action.DisplayInfo?.TitleFormat);
                    var actionNumberBinder = _modelBinderFactory.Create(action, action.DisplayInfo?.NumberFormat);

                    foreach (var testCase in controllerTestCases)
                    {
                        foreach (var testStep in GetTestSteps(testCase, action))
                        {
                            //-- Объединяем в иерархию
                            testCase.TestSteps.Add(testStep);
                            testStep.Parent = testStep;

                            FillDisplayName(action, actionTitleBinder, testStep);

                            var result = ControllerActionDescriptorBuilder.Flattener(application, controller, action, testCase, testStep);

                            Debug.Assert(result != null);

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }

        private static void FillDisplayName(ActionModel action, IModelBinder modelBinder, ControllerTestStep testStep)
        {
            if (modelBinder != null)
            {
                var bindingResult = modelBinder.Bind(testStep);
                if (bindingResult.IsModelSet)
                {
                    //TODO: Проверка Model на нуль
                    testStep.Title = bindingResult.Model.ToString();
                    return;
                }

                //TODO: Добавить проверку результата паривязки
                
            }

            testStep.Title = action.DisplayInfo?.TitleFormat ?? action.ActionName ?? action.ActionMethod.Name;


            //if (ActionModel.NumberBinder != null)
            //{
            //    var bindingResult = ActionModel.NumberBinder.Bind(this);
            //    if (bindingResult.IsModelSet)
            //        return bindingResult.Model.ToString();
            //    else
            //        return "Ошибка привязки";
            //}

            //return ActionModel.DisplayInfo?.NumberFormat
            //    ?? (IndexSecond.HasValue ? string.Format("{0}.{1}", Index, IndexSecond.Value) : Index.ToString());
        }
        
        private void FillDisplayInfo(ControllerModel controller, ControllerTestCase[] controllerTestCases)
        {
            if (controller.DisplayInfo == null)
                return;

            var controllerTitleBinder  = _modelBinderFactory.Create(controller, controller.DisplayInfo.TitleFormat);
            var controllerNumberBinder = _modelBinderFactory.Create(controller, controller.DisplayInfo.NumberFormat);

            foreach (var testCase in controllerTestCases)
            {
                if (controllerTitleBinder == null)
                {
                    testCase.Title = controller.DisplayInfo.TitleFormat ?? controller.ControllerName;
                }
                else
                {
                    var bindingResult = controllerTitleBinder.Bind(testCase);
                    if (bindingResult.IsModelSet)
                        testCase.Title = bindingResult.Model.ToString();
                    else
                        testCase.Title = "Ошибка привязки";
                }
            }
        }

        

        private IEnumerable<ControllerTestCase> GetTestCases(ControllerModel controllerModel)
        {
            var modelBinder = _modelBinderFactory.Create(controllerModel);
            if (modelBinder == null)
            {
                yield return new ControllerTestCase(controllerModel);
                yield break;
            }

            //-- 2) Не получилось привязаться к данным? пропускаем. bindingResult
            var resultBinding = modelBinder.Bind(null, 1);
            if (!resultBinding.IsModelSet)
            {
                logger.LogError(
                    resultBinding.Exception,
                    $"Не получилось привязаться к данным. " +
                    $"Контроллер: {controllerModel.DisplayName};");

                yield break;
            }

            //-- 3) привязка вернула null данным? пропускаем.
            if (resultBinding.IsModelNull)
            {
                logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Контроллер: {controllerModel.DisplayName};");

                yield break;
            }

            //-- 4) Количество данных равно единице? возвращаем один TestCase
            if (!resultBinding.IsMany)
            {
                var testCase = new ControllerTestCase(controllerModel)
                {
                    DataContext = resultBinding.Model
                };

                yield return testCase;
                yield break;
            }

            //-- 5) Массив
            var bindingResults = resultBinding.GetResults();
            if (bindingResults.Length == 0)
                logger.LogWarning(
                    $"Привязка к пустой коллекции. " +
                    $"Контроллер: {controllerModel.DisplayName};");

            for (int i = 0, index = _startIndex; i < bindingResults.Length; i++, index++)
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
                    $"Index:{i}; Контроллер: {controllerModel.DisplayName};");

                    continue;
                }

                if (bindingResult.IsModelNull)
                {
                    logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Index:{i}; Контроллер: {controllerModel.DisplayName};");

                    continue;
                }

                var testCase = new ControllerTestCase(controllerModel, index)
                {
                    DataContext = resultBinding.Model
                };
                yield return testCase;
            }
        }

        private IEnumerable<ControllerTestStep> GetTestSteps(ActionDescriptorObject testCase, ActionModel actionModel)
        {
            var modelBinder = _modelBinderFactory.Create(actionModel);

            //-- 1) Нету привязки? создаем одиночный TestStep
            if (modelBinder == null)
            {
                yield return new ControllerTestStep(actionModel);
                yield break;
            }

            //-- 2) Не получилось привязаться к данным? пропускаем.
            var resultBinding = modelBinder.Bind(testCase, 1);
            if (!resultBinding.IsModelSet)
            {
                logger.LogError(
                    resultBinding.Exception,
                    $"Не получилось привязаться к данным. " +
                    $"Метод: {actionModel.DisplayName};");

                yield break;
            }

            //-- 3) привязка вернула null данным? пропускаем.
            if (resultBinding.IsModelNull)
            {
                logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Метод: {actionModel.DisplayName};");

                yield break;
            }

            //-- 4) Количество данных равно единице? возвращаем один TestStep
            if (!resultBinding.IsMany)
            {
                var testStep = new ControllerTestStep(actionModel);
                testStep.DataContext = resultBinding.Model;

                yield return testStep;
                yield break;
            }

            //-- 5) Массив
            var bindingResults = resultBinding.GetResults();
            if (bindingResults.Length == 0)
                logger.LogWarning(
                    $"Привязка к пустой коллекции. " +
                    $"Метод: {actionModel.DisplayName};");


            for (int i = 0, index = _startIndex; i < bindingResults.Length; i++, index++)
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
                        $"Метод: {actionModel.DisplayName};");

                    continue;
                }

                if (bindingResult.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Index:{i}; Метод: {actionModel.DisplayName};");

                    continue;
                }


                var testStep = new ControllerTestStep(actionModel, index);
                testStep.DataContext = bindingResult.Model;

                yield return testStep;
            }
        }

        
    }
}
