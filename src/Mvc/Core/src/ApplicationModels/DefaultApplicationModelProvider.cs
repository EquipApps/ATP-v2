using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.Routing;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc.ModelBinding;
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
        private readonly MvcOptions _mvcOptions;
        private readonly IModelMetadataProvider _modelMetadataProvider;      
        private readonly IModelBindingFactory _modelBinderFactory;

        public DefaultApplicationModelProvider(
            IOptions<MvcOptions> mvcOptionsAccessor,
            IModelMetadataProvider modelMetadataProvider,
            IModelBindingFactory modelBinderFactory)
        {
            _mvcOptions            = mvcOptionsAccessor.Value;
            _modelMetadataProvider = modelMetadataProvider;
            _modelBinderFactory = modelBinderFactory;
        }

        public int Order => -1000;

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var controllerType in context.ControllerTypes)
            {
                var controllerModel = CreateControllerModel(controllerType);
                if (controllerModel == null)
                {
                    continue;
                }

                context.Result.Controllers.Add(controllerModel);
                controllerModel.Application = context.Result;

                foreach (var propertyHelper in PropertyHelper.GetProperties(controllerType.AsType()))
                {
                    var propertyInfo = propertyHelper.Property;
                    var propertyModel = CreatePropertyModel(propertyInfo);
                    if (propertyModel != null)
                    {
                        propertyModel.Controller = controllerModel;
                        controllerModel.ControllerProperties.Add(propertyModel);
                    }
                }

                foreach (var methodInfo in controllerType.AsType().GetMethods())
                {
                    var actionModel = CreateActionModel(controllerType, methodInfo);
                    if (actionModel == null)
                    {
                        continue;
                    }

                    actionModel.Controller = controllerModel;
                    controllerModel.Actions.Add(actionModel);

                    foreach (var parameterInfo in actionModel.ActionMethod.GetParameters())
                    {
                        var parameterModel = CreateParameterModel(parameterInfo);
                        if (parameterModel != null)
                        {
                            parameterModel.Action = actionModel;
                            actionModel.Parameters.Add(parameterModel);
                        }
                    }
                }
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            //--
        }

        

        /// <summary>
        /// Creates a <see cref="ControllerModel"/> for the given <see cref="TypeInfo"/>.
        /// </summary>
        internal ControllerModel CreateControllerModel(TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            // CoreCLR returns IEnumerable<Attribute> from GetCustomAttributes - the OfType<object>
            // is needed to so that the result of ToArray() is object
            var attributes = typeInfo.GetCustomAttributes(inherit: true);

            //-- Конфигурация BindingInfo
            var bindingInfo = BindingInfo.GetBindingInfo(attributes);
            if (bindingInfo == null)
            {
                //-- Создание BindingInfo через атрибут IModelExpected<>
                var modelExpectedType = typeInfo.ImplementedInterfaces
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IModelExpected<>));

                if (modelExpectedType != null)
                {
                    bindingInfo = new BindingInfo();
                    bindingInfo.BindingModelType = modelExpectedType.GenericTypeArguments.FirstOrDefault();
                    bindingInfo.BindingSource    = BindingSource.ModelProvider;
                }
            }

            //-- Конфигурация DisplayInfo
            var displayInfo = DisplayInfo.GetDisplayInfo(attributes);

            var controllerModel = new ControllerModel(typeInfo, attributes)
            {
                DisplayInfo = displayInfo,
                BindingInfo = bindingInfo,
            };


            controllerModel.ControllerName = typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ?
                    typeInfo.Name.Substring(0, typeInfo.Name.Length - "Controller".Length) :
                    typeInfo.Name;



            foreach (var routeValueProvider in attributes.OfType<IRouteValueProvider>())
            {
                controllerModel.RouteValues.Add(routeValueProvider.RouteKey, routeValueProvider.RouteValue);
            }

            foreach (var orderValueProvider in attributes.OfType<IOrderValueProvider>())
            {
                controllerModel.OrderValues.Add(orderValueProvider.OrderKey, orderValueProvider.OrderValue);
            }


            return controllerModel;
        }

        /// <summary>
        /// Creates a <see cref="PropertyModel"/> for the given <see cref="PropertyInfo"/>.
        /// </summary>       
        internal PropertyModel CreatePropertyModel(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var attributes = propertyInfo.GetCustomAttributes(inherit: true);

            // BindingInfo for properties can be either specified by decorating the property with binding specific attributes.
            // ModelMetadata also adds information from the property's type and any configured IBindingMetadataProvider.
            var modelMetadata = _modelMetadataProvider.GetMetadataForProperty(propertyInfo.DeclaringType, propertyInfo.Name);
            var bindingInfo = BindingInfo.GetBindingInfo(attributes, modelMetadata);

            if (bindingInfo == null)
            {
                // Look for BindPropertiesAttribute on the handler type if no BindingInfo was inferred for the property.
                // This allows a user to enable model binding on properties by decorating the controller type with BindPropertiesAttribute.
                var declaringType = propertyInfo.DeclaringType;
                var bindPropertiesAttribute = declaringType.GetCustomAttribute<BindPropertiesAttribute>(inherit: true);
                if (bindPropertiesAttribute != null)
                {
                    bindingInfo = new BindingInfo();
                }
            }

            if (bindingInfo != null)
            {
                /*
                 * Если Имя модели NULL      => Привязка по имени свойства
                 * Если Тип модели NULL      => Привязка по типу свойства
                 * Если Источник модели NULL => Привязка из DataContext
                 */
                if (bindingInfo.BindingModelName == null) bindingInfo.BindingModelName = propertyInfo.Name;
                if (bindingInfo.BindingModelType == null) bindingInfo.BindingModelType = propertyInfo.PropertyType;
                if (bindingInfo.BindingSource == null)    bindingInfo.BindingSource = BindingSource.DataContext;
            }

            var propertyModel = new PropertyModel(propertyInfo, attributes)
            {
                PropertyName = propertyInfo.Name,
                BindingInfo  = bindingInfo
            };




            return propertyModel;
        }

        /// <summary>
        /// Creates the <see cref="ActionModel"/> instance for the given action <see cref="MethodInfo"/>.
        /// </summary>       
        internal ActionModel CreateActionModel(
            TypeInfo typeInfo,
            MethodInfo methodInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!IsAction(typeInfo, methodInfo))
            {
                return null;
            }

            // CoreCLR returns IEnumerable<Attribute> from GetCustomAttributes - the OfType<object>
            // is needed to so that the result of ToArray() is object
            var attributes = methodInfo.GetCustomAttributes(inherit: true);

            var actionModel = new ActionModel(methodInfo, attributes);

            var actionName = attributes.OfType<ActionNameAttribute>().FirstOrDefault();
            if (actionName?.Name != null)
            {
                actionModel.ActionName = actionName.Name;
            }
            else
            {
                actionModel.ActionName = CanonicalizeActionName(methodInfo.Name);
            }

            actionModel.BindingInfo = BindingInfo.GetBindingInfo(attributes);
            actionModel.DisplayInfo = DisplayInfo.GetDisplayInfo(attributes);

            foreach (var routeValueProvider in attributes.OfType<IRouteValueProvider>())
            {
                actionModel.RouteValues.Add(routeValueProvider.RouteKey, routeValueProvider.RouteValue);
            }

            foreach (var orderValueProvider in attributes.OfType<IOrderValueProvider>())
            {
                actionModel.OrderValues.Add(orderValueProvider.OrderKey, orderValueProvider.OrderValue);
            }

            return actionModel;
        }

        private string CanonicalizeActionName(string actionName)
        {
            const string Suffix = "Async";

            if (_mvcOptions.SuppressAsyncSuffixInActionNames &&
                actionName.EndsWith(Suffix, StringComparison.Ordinal))
            {
                actionName = actionName.Substring(0, actionName.Length - Suffix.Length);
            }

            return actionName;
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="methodInfo"/> is an action. Otherwise <c>false</c>.
        /// </summary>       
        /// <remarks>
        /// Override this method to provide custom logic to determine which methods are considered actions.
        /// </remarks>
        internal bool IsAction(TypeInfo typeInfo, MethodInfo methodInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

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
            if (methodInfo.GetBaseDefinition().DeclaringType == typeof(object))
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

        /// <summary>
        /// Creates a <see cref="ParameterModel"/> for the given <see cref="ParameterInfo"/>.
        /// </summary>       
        internal ParameterModel CreateParameterModel(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            var attributes = parameterInfo.GetCustomAttributes(inherit: true);

            BindingInfo bindingInfo;
            if (_modelMetadataProvider is ModelMetadataProvider modelMetadataProviderBase)
            {
                var modelMetadata = modelMetadataProviderBase.GetMetadataForParameter(parameterInfo);
                bindingInfo = BindingInfo.GetBindingInfo(attributes, modelMetadata);
            }
            else
            {
                // GetMetadataForParameter should only be used if the user has opted in to the 2.1 behavior.
                bindingInfo = BindingInfo.GetBindingInfo(attributes);
            }

            if (bindingInfo != null)
            {
                /*
                * Если Имя модели NULL      => Привязка по имени параметра
                * Если Тип модели NULL      => Привязка по типу параметра
                * Если Источник модели NULL => Привязка из DataContext
                */
                if (bindingInfo.BindingModelName == null) bindingInfo.BindingModelName = parameterInfo.Name;
                if (bindingInfo.BindingModelType == null) bindingInfo.BindingModelType = parameterInfo.ParameterType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }

            //-- Все параметры должны иметь привязку.
            //-- Если привязка не создана, делаем поумолчанию
            if (bindingInfo == null)
            {
                bindingInfo = new BindingInfo();
                bindingInfo.BindingModelName = parameterInfo.Name;
                bindingInfo.BindingModelType = parameterInfo.ParameterType;
                bindingInfo.BindingSource = BindingSource.DataContext;
            }

            var parameterModel = new ParameterModel(parameterInfo, attributes)
            {
                ParameterName = parameterInfo.Name,
                BindingInfo   = bindingInfo
            };

            return parameterModel;
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

        

        

        private static void AddRange<T>(IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
