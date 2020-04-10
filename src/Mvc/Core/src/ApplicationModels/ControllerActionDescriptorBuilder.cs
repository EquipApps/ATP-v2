using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Controllers;
using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EquipApps.Mvc.ApplicationModels
{
    public class BinderItem
    {
        public BinderItem(IModelBinder modelBinder, ModelMetadata modelMetadata)
        {
            ModelBinder = modelBinder;
            ModelMetadata = modelMetadata;
        }

        public IModelBinder ModelBinder { get; }

        public ModelMetadata ModelMetadata { get; }
    }

    public class ControllerActionDescriptorBuilder
    {
        private IModelBinderFactory _modelBinderFactory;
        private IModelMetadataProvider _modelMetadataProvider;
        private ILogger<ControllerActionDescriptorBuilder> logger;
        private readonly int _startIndex = 0;

        public ControllerActionDescriptorBuilder(IOptions<MvcOptions> options,
            IModelBinderFactory modelBinderFactory,
            IModelMetadataProvider modelMetadataProvider,
            ILoggerFactory loggerFactory)
        {
            _modelBinderFactory = modelBinderFactory ?? throw new ArgumentNullException(nameof(modelBinderFactory));
            _modelMetadataProvider = modelMetadataProvider ?? throw new ArgumentNullException(nameof(modelMetadataProvider));

            logger = loggerFactory.CreateLogger<ControllerActionDescriptorBuilder>();

            _startIndex = options.Value.StartIndex;
        }

        public IList<ControllerActionDescriptor> Builder(ApplicationModel application)
        {
            var results = new List<ControllerActionDescriptor>();

            foreach (var controller in application.Controllers)
            {
                var boundBinderItem = GetPropertyBindingInfo(_modelBinderFactory, _modelMetadataProvider, controller);
                var testCases       = GetTestCases(controller).ToList();

                foreach (var testCase in testCases)
                {
                    FillTitle(_modelBinderFactory, controller, testCase);
                }

                foreach (var action in controller.Actions)
                {
                    var binderItem = GetParameterBindingInfo(_modelBinderFactory, _modelMetadataProvider, action);

                    foreach (var testCase in testCases)
                    {
                        foreach (var testStep in GetTestSteps(testCase, action))
                        {
                            testCase.TestSteps.Add(testStep);
                            testStep.Parent = testStep;

                            FillTitle(_modelBinderFactory, action, testStep);

                            var result = ControllerActionDescriptorBuilder.Flattener(
                                application, controller, action, testCase, testStep);

                                result.BoundBinderItem = boundBinderItem;
                                result.BinderItem      = binderItem;


                            Debug.Assert(result != null);

                            results.Add(result);
                        }    
                    }
                }
            }

            return results;
        }

        private static BinderItem[] GetParameterBindingInfo(
            IModelBinderFactory modelBinderFactory,
            IModelMetadataProvider modelMetadataProvider,
            ActionModel actionModel)
        {
            var parameters = actionModel.Parameters
                .Where(p => p.BindingInfo != null)
                .ToList();

            if (parameters.Count == 0)
            {
                return null;
            }

            var parameterBindingInfo = new BinderItem[parameters.Count];
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];

                ModelMetadata metadata;
                if (modelMetadataProvider is ModelMetadataProvider modelMetadataProviderBase)
                {
                    // The default model metadata provider derives from ModelMetadataProvider
                    // and can therefore supply information about attributes applied to parameters.
                    metadata = modelMetadataProviderBase.GetMetadataForParameter(parameter.ParameterInfo);
                }
                else
                {
                    // For backward compatibility, if there's a custom model metadata provider that
                    // only implements the older IModelMetadataProvider interface, access the more
                    // limited metadata information it supplies. In this scenario, validation attributes
                    // are not supported on parameters.
                    metadata = modelMetadataProvider.GetMetadataForType(parameter.ParameterType);
                }

                var binder = modelBinderFactory.Create(parameter);

                Debug.Assert(binder != null, $"{actionModel.DisplayName}; {parameter.Name};");

                parameterBindingInfo[i] = new BinderItem(binder, metadata);
            }

            return parameterBindingInfo;
        }

        private static BinderItem[] GetPropertyBindingInfo(
            IModelBinderFactory modelBinderFactory,
            IModelMetadataProvider modelMetadataProvider, 
            ControllerModel controllerModel)
        {
            var properties = controllerModel.ControllerProperties
                .Where(p => p.BindingInfo != null)                
                .ToList();

            if (properties.Count == 0)
            {
                return null;
            }

            var propertyBindingInfo = new BinderItem[properties.Count];
            var controllerType = controllerModel.ControllerType;

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var metadata = modelMetadataProvider.GetMetadataForProperty(controllerType, property.Name);
                var binder   = modelBinderFactory.Create(property);

                Debug.Assert(binder != null,
                    "Привязка не создана!",
                    "Controller: {0};\nProperty: {1};", controllerModel.DisplayName, property.Name);
                  

                propertyBindingInfo[i] = new BinderItem(binder, metadata);
            }

            return propertyBindingInfo;

        }

        internal static void FillTitle (
            IModelBinderFactory modelBinderFactory, 
            IDisplayModel displayModel, 
            ActionDescriptorObject descriptorObject)
        {
            var displayInfo = displayModel.DisplayInfo;
            if (displayInfo != null)
            {
                if (displayInfo.TitleFormat != null)
                {
                    var titleBinder = modelBinderFactory.Create(displayModel, displayInfo.TitleFormat);
                    if (titleBinder != null)
                    {
                        var bindingResult = titleBinder.Bind(descriptorObject);
                        if (bindingResult.IsModelSet)
                        {
                            //TODO: Проверка Model на нуль
                            descriptorObject.Title = bindingResult.Model.ToString();
                            return;
                        }

                        //TODO: Добавить проверку результата паривязки
                    }
                }
            }
 
            descriptorObject.Title = displayModel.DisplayInfo?.TitleFormat ??
                                     displayModel.Name ??
                                     displayModel.MemberInfo.Name;
        }

        internal static void FillNumber(IDisplayModel displayModel, ActionDescriptorObject descriptorObject)
        {
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

        






        private IEnumerable<ControllerTestCase> GetTestCases(ControllerModel controllerModel)
        {
            var modelBinder = _modelBinderFactory.Create(controllerModel);
            if (modelBinder == null)
            {
                yield return new ControllerTestCase();
                yield break;
            }

            //-- 2) Не получилось привязаться к данным? пропускаем. bindingResult
            var result = modelBinder.Bind(null, 1);
            if (!result.IsModelSet)
            {
                logger.LogError(
                    result.Exception,
                    $"Не получилось привязаться к данным. " +
                    $"Контроллер: {controllerModel.DisplayName};");

                yield break;
            }

            //-- 3) привязка вернула null данным? пропускаем.
            if (result.IsModelNull)
            {
                logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Контроллер: {controllerModel.DisplayName};");

                yield break;
            }

            //-- 4) Количество данных равно единице? возвращаем один TestCase
            if (!result.IsMany)
            {
                var testCase = new ControllerTestCase()
                {
                    DataContext = result.Model
                };

                yield return testCase;
                yield break;
            }

            //-- 5) Массив
            var resultArray = result.GetResults();
            if (resultArray.Length == 0)
                logger.LogWarning(
                    $"Привязка к пустой коллекции. " +
                    $"Контроллер: {controllerModel.DisplayName};");

            for (int i = 0, index = _startIndex; i < resultArray.Length; i++, index++)
            {
                /*  
                 *  Если результат не удалось получить, то пропускаем данный пункт проверки! (НО ИНДЕКС УВЕЛИЧИВАЕТСЯ)
                 *  Нужно для маскирования ПУСТЫХ СТРОК в базе данных!
                 */
                var resultItem = resultArray[i];

                if (!resultItem.IsModelSet)
                {
                    logger.LogError(
                    result.Exception,
                    $"Не получилось привязаться к данным. " +
                    $"Index:{i}; Контроллер: {controllerModel.DisplayName};");

                    continue;
                }

                if (resultItem.IsModelNull)
                {
                    logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Index:{i}; Контроллер: {controllerModel.DisplayName};");

                    continue;
                }

                var testCase = new ControllerTestCase(index)
                {
                    DataContext = resultItem.Model
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
                yield return new ControllerTestStep();
                yield break;
            }

            //-- 2) Не получилось привязаться к данным? пропускаем.
            var result = modelBinder.Bind(testCase, 1);
            if (!result.IsModelSet)
            {
                logger.LogError(
                    result.Exception,
                    $"Не получилось привязаться к данным. " +
                    $"Метод: {actionModel.DisplayName};");

                yield break;
            }

            //-- 3) привязка вернула null данным? пропускаем.
            if (result.IsModelNull)
            {
                logger.LogWarning(
                    $"Модель привязки NULL. " +
                    $"Метод: {actionModel.DisplayName};");

                yield break;
            }

            //-- 4) Количество данных равно единице? возвращаем один TestStep
            if (!result.IsMany)
            {
                var testStep = new ControllerTestStep();
                testStep.DataContext = result.Model;

                yield return testStep;
                yield break;
            }

            //-- 5) Массив
            var resultArray = result.GetResults();
            if (resultArray.Length == 0)
                logger.LogWarning(
                    $"Привязка к пустой коллекции. " +
                    $"Метод: {actionModel.DisplayName};");


            for (int i = 0, index = _startIndex; i < resultArray.Length; i++, index++)
            {
                var resultItem = resultArray[i];

                /*
                 * Если результат неудалось получить, то пропускаем данный пункт проверки! (НО ИНДЕКС УВЕЛИЧИВАЕТСЯ)
                 * Нужно для маскирования ПУСТЫХ СТРОК в базе данных!
                 */
                if (!resultItem.IsModelSet)
                {
                    logger.LogError(
                        resultItem.Exception,
                        $"Не получилось привязаться к данным. Index:{i}; " +
                        $"Метод: {actionModel.DisplayName};");

                    continue;
                }

                if (resultItem.IsModelNull)
                {
                    logger.LogWarning(
                        $"Модель привязки NULL. " +
                        $"Index:{i}; Метод: {actionModel.DisplayName};");

                    continue;
                }

                var testStep = new ControllerTestStep(index);
                    testStep.DataContext = resultItem.Model;

                yield return testStep;
            }
        }













        public static ControllerActionDescriptor Flattener(ApplicationModel application, ControllerModel controller, ActionModel action,
                                                           ControllerTestCase testCase,  ControllerTestStep testStep)
        {
            var actionDescriptor = new ControllerActionDescriptor(testCase, testStep)
            {
                ActionName = action.ActionName,
                MethodInfo = action.ActionMethod,
            };

            actionDescriptor.ControllerName = controller.ControllerName;
            actionDescriptor.ControllerTypeInfo = controller.ControllerType;
            AddControllerPropertyDescriptors(actionDescriptor, controller);

            AddParameterDescriptors(actionDescriptor, action);

            AddRouteValues(actionDescriptor, controller, action);
            AddOrderValues(actionDescriptor, controller, action, testCase, testStep);
            AddProperties (actionDescriptor, action, controller, application);

            actionDescriptor.Number = new TestNumberBuilder()
                .Append(actionDescriptor.OrderValues["controller"])
                .Append(actionDescriptor.OrderValues["controller_bind"])
                .Append(actionDescriptor.OrderValues["action"])
                .Append(actionDescriptor.OrderValues["action_bind"])
                .Build();

            return actionDescriptor;
        }

        private static void AddControllerPropertyDescriptors(ActionDescriptor actionDescriptor, ControllerModel controller)
        {
            actionDescriptor.BoundProperties = controller.ControllerProperties
                .Where(p => p.BindingInfo != null)
                .Select(CreateParameterDescriptor)
                .ToList();
        }
        private static void AddParameterDescriptors(ActionDescriptor actionDescriptor, ActionModel action)
        {
            var parameterDescriptors = new List<ParameterDescriptor>();
            foreach (var parameter in action.Parameters)
            {
                var parameterDescriptor = CreateParameterDescriptor(parameter);
                parameterDescriptors.Add(parameterDescriptor);
            }

            actionDescriptor.Parameters = parameterDescriptors;
        }
        private static ParameterDescriptor CreateParameterDescriptor(ParameterModel parameterModel)
        {
            var parameterDescriptor = new ControllerParameterDescriptor()
            {
                Name = parameterModel.ParameterName,
                ParameterType = parameterModel.ParameterInfo.ParameterType,
                BindingInfo = parameterModel.BindingInfo,
                ParameterInfo = parameterModel.ParameterInfo,
            };

            return parameterDescriptor;
        }
        private static ParameterDescriptor CreateParameterDescriptor(PropertyModel propertyModel)
        {
            var parameterDescriptor = new ControllerBoundPropertyDescriptor()
            {
                BindingInfo = propertyModel.BindingInfo,
                Name = propertyModel.PropertyName,
                ParameterType = propertyModel.PropertyInfo.PropertyType,
                PropertyInfo = propertyModel.PropertyInfo,
            };

            return parameterDescriptor;
        }


        private static void AddProperties(
           ControllerActionDescriptor actionDescriptor,
           ActionModel action,
           ControllerModel controller,
           ApplicationModel application)
        {
            foreach (var item in application.Properties)
            {
                actionDescriptor.Properties[item.Key] = item.Value;
            }

            foreach (var item in controller.Properties)
            {
                actionDescriptor.Properties[item.Key] = item.Value;
            }

            foreach (var item in action.Properties)
            {
                actionDescriptor.Properties[item.Key] = item.Value;
            }
        }

        private static void AddRouteValues(
            ControllerActionDescriptor actionDescriptor,
            ControllerModel controller,
            ActionModel action)
        {
            // Apply all the constraints defined on the action, then controller (for example, [Area])
            // to the actions. Also keep track of all the constraints that require preventing actions
            // without the constraint to match. For example, actions without an [Area] attribute on their
            // controller should not match when a value has been given for area when matching a url or
            // generating a link.
            foreach (var kvp in action.RouteValues)
            {
                // Skip duplicates
                if (!actionDescriptor.RouteValues.ContainsKey(kvp.Key))
                {
                    actionDescriptor.RouteValues.Add(kvp.Key, kvp.Value);
                }
            }

            foreach (var kvp in controller.RouteValues)
            {
                // Skip duplicates - this also means that a value on the action will take precedence
                if (!actionDescriptor.RouteValues.ContainsKey(kvp.Key))
                {
                    actionDescriptor.RouteValues.Add(kvp.Key, kvp.Value);
                }
            }

            // Lastly add the 'default' values
            if (!actionDescriptor.RouteValues.ContainsKey("action"))
            {
                actionDescriptor.RouteValues.Add("action", action.ActionName ?? string.Empty);
            }

            if (!actionDescriptor.RouteValues.ContainsKey("controller"))
            {
                actionDescriptor.RouteValues.Add("controller", controller.ControllerName);
            }
        }

        private static void AddOrderValues(ControllerActionDescriptor actionDescriptor,
                                           ControllerModel controller, ActionModel action,
                                           ControllerTestCase testCase, ControllerTestStep testStep)
        {
            // Apply all the constraints defined on the action, then controller (for example, [Area])
            // to the actions. Also keep track of all the constraints that require preventing actions
            // without the constraint to match. For example, actions without an [Area] attribute on their
            // controller should not match when a value has been given for area when matching a url or
            // generating a link.
            foreach (var kvp in action.OrderValues)
            {
                // Skip duplicates
                if (!actionDescriptor.OrderValues.ContainsKey(kvp.Key))
                {
                    actionDescriptor.OrderValues.Add(kvp.Key, kvp.Value);
                }
            }

            foreach (var kvp in controller.OrderValues)
            {
                // Skip duplicates - this also means that a value on the action will take precedence
                if (!actionDescriptor.OrderValues.ContainsKey(kvp.Key))
                {
                    actionDescriptor.OrderValues.Add(kvp.Key, kvp.Value);
                }
            }

            // Lastly add the 'default' values
            if (!actionDescriptor.OrderValues.ContainsKey("controller"))
            {
                actionDescriptor.OrderValues.Add("controller", null);
            }

            if (!actionDescriptor.OrderValues.ContainsKey("controller_bind"))
            {
                actionDescriptor.OrderValues.Add("controller_bind", testCase.BindIndex?.ToString());
            }

            if (!actionDescriptor.OrderValues.ContainsKey("action"))
            {
                actionDescriptor.OrderValues.Add("action", null);
            }

            if (!actionDescriptor.OrderValues.ContainsKey("action_bind"))
            {
                actionDescriptor.OrderValues.Add("action_bind", testStep.BindIndex?.ToString());
            }
        }
    }
}
