using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ApplicationParts;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Testing.Mvc.Internal;
using NLib.AtpNetCore.Testing.Mvc.ModelBinding.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NLib.AtpNetCore.Mvc.Internal
{
    /// <summary>
    /// Провайдер <see cref="ApplicationModel"/> (по умолчанию).
    /// </summary>
    public class MvcApplicationModelProvider : IApplicationModelProvider
    {
        private readonly ApplicationPartManager _applicationManager;
        private readonly IBindingFactory _modelBinderFactory;

        public MvcApplicationModelProvider(
            ApplicationPartManager applicationManager,
            IBindingFactory modelBinderFactory)
        {
            _applicationManager = applicationManager ?? throw new ArgumentNullException(nameof(applicationManager));
            _modelBinderFactory = modelBinderFactory ?? throw new ArgumentNullException(nameof(modelBinderFactory));
        }

        public int Order => -1000;



        public void OnProvidersExecuting(ApplicationModelContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var application = context.Result;


            foreach (var controllerType in GetControllerTypes())
            {
                //-- КОНТРОЛЛЕР
                var controllerModel = MvcApplicationModelBuilder.CreateControllerModel(controllerType);
                if (controllerModel == null)
                {
                    continue;
                }

                controllerModel.Application = application;
                application.Controllers.Add(controllerModel);

                //-- ФОНОВАЯ МОДЕЛЬ (ПРИВЯЗКА)
                var backgroundtype = controllerModel.BindingInfo?.ModelType?.GetTypeInfo();
                var backgroundModel = MvcApplicationModelBuilder.CreateBackgroundModel(backgroundtype);
                if (backgroundModel != null)
                {
                    backgroundModel.Controller = controllerModel;
                    controllerModel.Background = backgroundModel;
                }

                //-- СВОЙСТВА КОНТРОЛЛЕРА
                var propertyHelpers = PropertyHelper.GetVisibleProperties(controllerType);

                
                foreach (var propertyHelper in propertyHelpers)
                {
                    var propertyInfo  = propertyHelper.Property;
                    var propertyModel = MvcApplicationModelBuilder.CreatePropertyModel(propertyInfo);
                    if (propertyModel != null)
                    {
                        propertyModel.Controller = controllerModel;
                        controllerModel.Properties.Add(propertyModel);
                    }
                }

                //-- ФИЛЬТРАЦИЯ МЕТОДОВ
                var methodInfoCollection = controllerType.AsType().GetMethods()
                    .Where(IsAction)
                    .ToArray();

                //-- МЕТОДЫ КОНТРОЛЛЕРА
                foreach (var methodInfo in methodInfoCollection)
                {

                    var methodModel = MvcApplicationModelBuilder.CreateMethodModel(methodInfo);
                    if (methodModel == null)
                    {
                        continue;
                    }

                    //--
                    methodModel.Controller = controllerModel;
                    controllerModel.Methods.Add(methodModel);

                    //-- ПАРАМЕТРЫ
                    foreach (var parameterInfo in methodModel.Info.GetParameters())
                    {
                        var parameterModel = MvcApplicationModelBuilder.CreateParameterModel(parameterInfo);

                        if (parameterModel != null)
                        {
                            parameterModel.Method = methodModel;
                            methodModel.Parameters.Add(parameterModel);
                        }
                    }
                }
            }
        }

        public void OnProvidersExecuted(ApplicationModelContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var controllerModel in context.Result.Controllers)
            {
                BindingControllerModel(controllerModel);

                BindingBackgroundModel(controllerModel.Background);

                foreach (var propertyModel in controllerModel.Properties)
                {
                    BindingPropertyModel(propertyModel);
                }

                foreach (var actionModel in controllerModel.Methods)
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
                (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(declaringTypeInfo) &&
                 declaringTypeInfo.GetRuntimeInterfaceMap(typeof(IDisposable)).TargetMethods[0] == baseMethodInfo);
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

        private void BindingBackgroundModel(BackgroundModel backgroundModel)
        {
            //-- Нету, пропускаем!
            if (backgroundModel == null)
                return;

            if (backgroundModel.Title != null)
            {
                backgroundModel.TitleBinder = _modelBinderFactory.Create(backgroundModel.Controller, backgroundModel.Title);
            }

            if (backgroundModel.Number != null)
            {
                backgroundModel.NumberBinder = _modelBinderFactory.Create(backgroundModel.Controller, backgroundModel.Title);
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

        private void BindingActionModel(MethodModel actionModel)
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
                    .AppendLine(parameterModel.Method.Controller.Name)
                    .Append("Method: ")
                    .AppendLine(parameterModel.Method.Name)
                    .Append("Parameter: ")
                    .AppendLine(parameterModel.Name)
                    .ToString();

                throw new InvalidOperationException(error);
            }

            //-- Сохраняем!
            parameterModel.ModelBinder = modelBinder;
        }


        #endregion
    }
}
