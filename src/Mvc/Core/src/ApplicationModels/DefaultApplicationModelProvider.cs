using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Testing.Mvc.ModelBinding.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EquipApps.Mvc.Internal
{
    /// <summary>
    /// Провайдер <see cref="ApplicationModel"/> (по умолчанию).
    /// </summary>
    public class DefaultApplicationModelProvider : IApplicationModelProvider
    {
        private readonly ApplicationPartManager _applicationManager;
        private readonly IBindingFactory _modelBinderFactory;

        public DefaultApplicationModelProvider(
            ApplicationPartManager applicationManager,
            IBindingFactory modelBinderFactory)
        {
            _applicationManager = applicationManager ?? throw new ArgumentNullException(nameof(applicationManager));
            _modelBinderFactory = modelBinderFactory ?? throw new ArgumentNullException(nameof(modelBinderFactory));
        }

        public int Order => -1000;



        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var application = context.Result;


            foreach (var controllerType in GetControllerTypes())
            {
                //-- КОНТРОЛЛЕР
                var controllerModel = ApplicationModelBuilder.CreateControllerModel(controllerType);
                if (controllerModel == null)
                {
                    continue;
                }

                controllerModel.Application = application;
                application.Controllers.Add(controllerModel);


                //-- СВОЙСТВА КОНТРОЛЛЕРА
                var propertyHelpers = PropertyHelper.GetVisibleProperties(controllerType);

                foreach (var propertyHelper in propertyHelpers)
                {
                    var propertyInfo = propertyHelper.Property;
                    var propertyModel = ApplicationModelBuilder.CreatePropertyModel(propertyInfo);
                    if (propertyModel != null)
                    {
                        propertyModel.Controller = controllerModel;
                        controllerModel.ControllerProperties.Add(propertyModel);
                    }
                }

                //-- ФИЛЬТРАЦИЯ МЕТОДОВ
                var methodInfoCollection = controllerType.AsType().GetMethods()
                    .Where(IsAction)
                    .ToArray();

                //-- МЕТОДЫ КОНТРОЛЛЕРА
                foreach (var methodInfo in methodInfoCollection)
                {

                    var methodModel = ApplicationModelBuilder.CreateMethodModel(methodInfo);
                    if (methodModel == null)
                    {
                        continue;
                    }

                    //--
                    methodModel.Controller = controllerModel;
                    controllerModel.Actions.Add(methodModel);

                    //-- ПАРАМЕТРЫ
                    foreach (var parameterInfo in methodModel.Info.GetParameters())
                    {
                        var parameterModel = ApplicationModelBuilder.CreateParameterModel(parameterInfo);

                        if (parameterModel != null)
                        {
                            parameterModel.Action = methodModel;
                            methodModel.Parameters.Add(parameterModel);
                        }
                    }
                }
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var controllerModel in context.Result.Controllers)
            {
                BindingControllerModel(controllerModel);

                foreach (var propertyModel in controllerModel.ControllerProperties)
                {
                    BindingPropertyModel(propertyModel);
                }

                foreach (var actionModel in controllerModel.Actions)
                {
                    BindingActionModel(actionModel);

                    foreach (var parameterModel in actionModel.Parameters)
                    {
                        BindingParameterModel(parameterModel);
                    }
                }
            }
        }

        #region Create

        private IList<TypeInfo> GetControllerTypes()
        {
            //-- Создаем расширение.
            var feature = new ControllerFeature();

            _applicationManager.PopulateFeature(feature);

            return feature.Controllers;
        }

        private bool IsAction(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            // The SpecialName bit is set to flag members that are treated in a special way by some compilers
            // (such as property accessors and operator overloading methods).
            if (methodInfo.IsSpecialName)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(NonActionAttribute)))
            {
                return false;
            }

            // Overridden methods from Object class, e.g. Equals(Object), GetHashCode(), etc., are not valid.
            if (methodInfo.GetRuntimeBaseDefinition().DeclaringType == typeof(object))
            {
                return false;
            }

            // Dispose method implemented from IDisposable is not valid
            if (IsIDisposableMethod(methodInfo))
            {
                return false;
            }

            if (methodInfo.IsStatic)
            {
                return false;
            }

            if (methodInfo.IsAbstract)
            {
                return false;
            }

            if (methodInfo.IsConstructor)
            {
                return false;
            }

            if (methodInfo.IsGenericMethod)
            {
                return false;
            }

            return methodInfo.IsPublic;
        }

        private bool IsIDisposableMethod(MethodInfo methodInfo)
        {
            // Ideally we do not want Dispose method to be exposed as an action. However there are some scenarios where a user
            // might want to expose a method with name "Dispose" (even though they might not be really disposing resources)
            // Example: A controller deriving from MVC's Controller type might wish to have a method with name Dispose,
            // in which case they can use the "new" keyword to hide the base controller's declaration.

            // Find where the method was originally declared
            var baseMethodInfo = methodInfo.GetRuntimeBaseDefinition();
            var declaringTypeInfo = baseMethodInfo.DeclaringType.GetTypeInfo();

            return
                typeof(IDisposable).GetTypeInfo().IsAssignableFrom(declaringTypeInfo) &&
                 declaringTypeInfo.GetRuntimeInterfaceMap(typeof(IDisposable)).TargetMethods[0] == baseMethodInfo;
        }

        #endregion

        #region Bind

        private void BindingControllerModel(ControllerModel controllerModel)
        {
            var bindingInfo = controllerModel.BindingInfo;
            if (bindingInfo != null)
            {
                controllerModel.ModelBinder = _modelBinderFactory.Create(controllerModel);
            }

            if (controllerModel.Title != null)
            {
                controllerModel.TitleBinder = _modelBinderFactory.Create(controllerModel, controllerModel.Title);
            }

            if (controllerModel.Number != null)
            {
                controllerModel.NumberBinder = _modelBinderFactory.Create(controllerModel, controllerModel.Number);
            }
        }

       


        private void BindingPropertyModel(PropertyModel propertyModel)
        {
            var bindingInfo = propertyModel.BindingInfo;
            if (bindingInfo != null)
            {
                propertyModel.ModelBinder = _modelBinderFactory.Create(propertyModel);
            }
        }

        private void BindingActionModel(ActionModel actionModel)
        {
            var bindingInfo = actionModel.BindingInfo;
            if (bindingInfo != null)
            {
                var modelBinder = _modelBinderFactory.Create(actionModel);
                if (modelBinder == null)
                {
                    var error = new StringBuilder()
                        .AppendLine("Ошибка - Не возможно создать привязку!")

                        .Append("Controller: ")
                        .AppendLine(actionModel.Controller.Name)
                        .Append("Method: ")
                        .AppendLine(actionModel.Name)
                        .ToString();

                    throw new InvalidOperationException(error);
                }

                actionModel.ModelBinder = modelBinder;
            }

            //--
            if (actionModel.Title != null)
            {
                actionModel.TitleBinder = _modelBinderFactory.Create(actionModel, actionModel.Title);
            }
            if (actionModel.Number != null)
            {
                actionModel.NumberBinder = _modelBinderFactory.Create(actionModel, actionModel.Number);
            }
        }

        private void BindingParameterModel(ParameterModel parameterModel)
        {
            var bindingInfo = parameterModel.BindingInfo;
            if (bindingInfo == null)
            {
                //-- Информация о привязке должна существовать!
                throw new InvalidOperationException("Ошибка модели!");
            }

            var modelBinder = _modelBinderFactory.Create(parameterModel);
            if (modelBinder == null)
            {
                var error = new StringBuilder()
                    .AppendLine("Ошибка - Не возможно создать привязку!")

                    .Append("Controller: ")
                    .AppendLine(parameterModel.Action.Controller.Name)
                    .Append("Method: ")
                    .AppendLine(parameterModel.Action.Name)
                    .Append("Parameter: ")
                    .AppendLine(parameterModel.ParameterName)
                    .ToString();

                throw new InvalidOperationException(error);
            }

            //-- Сохраняем!
            parameterModel.ModelBinder = modelBinder;
        }


        #endregion
    }
}
