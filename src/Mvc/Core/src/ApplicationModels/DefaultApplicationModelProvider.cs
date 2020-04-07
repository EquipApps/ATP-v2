using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ApplicationParts;
using EquipApps.Mvc.Controllers;
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
        private readonly Func<ActionContext, bool> _supportsAllRequests;
        private readonly Func<ActionContext, bool> _supportsNonGetRequests;

        private readonly IBindingFactory _modelBinderFactory;

        public DefaultApplicationModelProvider(
            IOptions<MvcOptions> mvcOptionsAccessor,
            IModelMetadataProvider modelMetadataProvider,
            IBindingFactory modelBinderFactory)
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

            //foreach (var filter in _mvcOptions.Filters)
            //{
            //    context.Result.Filters.Add(filter);
            //}

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


        /// <summary>
        /// Creates a <see cref="ControllerModel"/> for the given <see cref="TypeInfo"/>.
        /// </summary>
        /// <param name="typeInfo">The <see cref="TypeInfo"/>.</param>
        /// <returns>A <see cref="ControllerModel"/> for the given <see cref="TypeInfo"/>.</returns>
        internal ControllerModel CreateControllerModel(TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            // For attribute routes on a controller, we want to support 'overriding' routes on a derived
            // class. So we need to walk up the hierarchy looking for the first class to define routes.
            //
            // Then we want to 'filter' the set of attributes, so that only the effective routes apply.
            var currentTypeInfo = typeInfo;
            var objectTypeInfo = typeof(object).GetTypeInfo();

            IRouteTemplateProvider[] routeAttributes;

            do
            {
                routeAttributes = currentTypeInfo
                    .GetCustomAttributes(inherit: false)
                    .OfType<IRouteTemplateProvider>()
                    .ToArray();

                if (routeAttributes.Length > 0)
                {
                    // Found 1 or more route attributes.
                    break;
                }

                currentTypeInfo = currentTypeInfo.BaseType.GetTypeInfo();
            }
            while (currentTypeInfo != objectTypeInfo);

            // CoreCLR returns IEnumerable<Attribute> from GetCustomAttributes - the OfType<object>
            // is needed to so that the result of ToArray() is object
            var attributes = typeInfo.GetCustomAttributes(inherit: true);

            // This is fairly complicated so that we maintain referential equality between items in
            // ControllerModel.Attributes and ControllerModel.Attributes[*].Attribute.
            var filteredAttributes = new List<object>();
            foreach (var attribute in attributes)
            {
                if (attribute is IRouteTemplateProvider)
                {
                    // This attribute is a route-attribute, leave it out.
                }
                else
                {
                    filteredAttributes.Add(attribute);
                }
            }

            filteredAttributes.AddRange(routeAttributes);

            attributes = filteredAttributes.ToArray();

            var controllerModel = new ControllerModel(typeInfo, attributes);


            //AddRange(controllerModel.Selectors, CreateSelectors(attributes));

            controllerModel.ControllerName =
                typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ?
                    typeInfo.Name.Substring(0, typeInfo.Name.Length - "Controller".Length) :
                    typeInfo.Name;

            //AddRange(controllerModel.Filters, attributes.OfType<IFilterMetadata>());

            foreach (var routeValueProvider in attributes.OfType<IRouteValueProvider>())
            {
                controllerModel.RouteValues.Add(routeValueProvider.RouteKey, routeValueProvider.RouteValue);
            }

            foreach (var orderValueProvider in attributes.OfType<IOrderValueProvider>())
            {
                controllerModel.OrderValues.Add(orderValueProvider.OrderKey, orderValueProvider.OrderValue);
            }

            //var apiVisibility = attributes.OfType<IApiDescriptionVisibilityProvider>().FirstOrDefault();
            //if (apiVisibility != null)
            //{
            //    controllerModel.ApiExplorer.IsVisible = !apiVisibility.IgnoreApi;
            //}

            //var apiGroupName = attributes.OfType<IApiDescriptionGroupNameProvider>().FirstOrDefault();
            //if (apiGroupName != null)
            //{
            //    controllerModel.ApiExplorer.GroupName = apiGroupName.GroupName;
            //}

            // Controllers can implement action filter and result filter interfaces. We add
            // a special delegating filter implementation to the pipeline to handle it.
            //
            // This is needed because filters are instantiated before the controller.
            //if (typeof(IAsyncActionFilter).GetTypeInfo().IsAssignableFrom(typeInfo) ||
            //    typeof(IActionFilter).GetTypeInfo().IsAssignableFrom(typeInfo))
            //{
            //    controllerModel.Filters.Add(new ControllerActionFilter());
            //}
            //if (typeof(IAsyncResultFilter).GetTypeInfo().IsAssignableFrom(typeInfo) ||
            //    typeof(IResultFilter).GetTypeInfo().IsAssignableFrom(typeInfo))
            //{
            //    controllerModel.Filters.Add(new ControllerResultFilter());
            //}

            return controllerModel;
        }

        /// <summary>
        /// Creates a <see cref="PropertyModel"/> for the given <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>A <see cref="PropertyModel"/> for the given <see cref="PropertyInfo"/>.</returns>
        internal PropertyModel CreatePropertyModel(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var attributes = propertyInfo.GetCustomAttributes(inherit: true);

            var propertyModel = new PropertyModel(propertyInfo, attributes)
            {
                PropertyName = propertyInfo.Name,
            };

            return propertyModel;
        }

        /// <summary>
        /// Creates the <see cref="ActionModel"/> instance for the given action <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="typeInfo">The controller <see cref="TypeInfo"/>.</param>
        /// <param name="methodInfo">The action <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// An <see cref="ActionModel"/> instance for the given action <see cref="MethodInfo"/> or
        /// <c>null</c> if the <paramref name="methodInfo"/> does not represent an action.
        /// </returns>
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

            //AddRange(actionModel.Filters, attributes.OfType<IFilterMetadata>());

            var actionName = attributes.OfType<ActionNameAttribute>().FirstOrDefault();
            if (actionName?.Name != null)
            {
                actionModel.ActionName = actionName.Name;
            }
            else
            {
                actionModel.ActionName = CanonicalizeActionName(methodInfo.Name);
            }

            //var apiVisibility = attributes.OfType<IApiDescriptionVisibilityProvider>().FirstOrDefault();
            //if (apiVisibility != null)
            //{
            //    actionModel.ApiExplorer.IsVisible = !apiVisibility.IgnoreApi;
            //}

            //var apiGroupName = attributes.OfType<IApiDescriptionGroupNameProvider>().FirstOrDefault();
            //if (apiGroupName != null)
            //{
            //    actionModel.ApiExplorer.GroupName = apiGroupName.GroupName;
            //}

            foreach (var routeValueProvider in attributes.OfType<IRouteValueProvider>())
            {
                actionModel.RouteValues.Add(routeValueProvider.RouteKey, routeValueProvider.RouteValue);
            }

            // Now we need to determine the action selection info (cross-section of routes and constraints)
            //
            // For attribute routes on a action, we want to support 'overriding' routes on a
            // virtual method, but allow 'overriding'. So we need to walk up the hierarchy looking
            // for the first definition to define routes.
            //
            // Then we want to 'filter' the set of attributes, so that only the effective routes apply.
            var currentMethodInfo = methodInfo;

            IRouteTemplateProvider[] routeAttributes;

            while (true)
            {
                routeAttributes = currentMethodInfo
                    .GetCustomAttributes(inherit: false)
                    .OfType<IRouteTemplateProvider>()
                    .ToArray();

                if (routeAttributes.Length > 0)
                {
                    // Found 1 or more route attributes.
                    break;
                }

                // GetBaseDefinition returns 'this' when it gets to the bottom of the chain.
                var nextMethodInfo = currentMethodInfo.GetBaseDefinition();
                if (currentMethodInfo == nextMethodInfo)
                {
                    break;
                }

                currentMethodInfo = nextMethodInfo;
            }

            // This is fairly complicated so that we maintain referential equality between items in
            // ActionModel.Attributes and ActionModel.Attributes[*].Attribute.
            var applicableAttributes = new List<object>();
            foreach (var attribute in attributes)
            {
                if (attribute is IRouteTemplateProvider)
                {
                    // This attribute is a route-attribute, leave it out.
                }
                else
                {
                    applicableAttributes.Add(attribute);
                }
            }

            applicableAttributes.AddRange(routeAttributes);
            //AddRange(actionModel.Selectors, CreateSelectors(applicableAttributes));

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
        /// <param name="typeInfo">The <see cref="TypeInfo"/>.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/>.</param>
        /// <returns><c>true</c> if the <paramref name="methodInfo"/> is an action. Otherwise <c>false</c>.</returns>
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
        /// <param name="parameterInfo">The <see cref="ParameterInfo"/>.</param>
        /// <returns>A <see cref="ParameterModel"/> for the given <see cref="ParameterInfo"/>.</returns>
        internal ParameterModel CreateParameterModel(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            var attributes = parameterInfo.GetCustomAttributes(inherit: true);

            var parameterModel = new ParameterModel(parameterInfo, attributes)
            {
                ParameterName = parameterInfo.Name,
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
                        .AppendLine(actionModel.Controller.ControllerName)
                        .Append("Method: ")
                        .AppendLine(actionModel.ActionName)
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
                return;
                throw new InvalidOperationException("Ошибка модели!");
            }

            var modelBinder = _modelBinderFactory.Create(parameterModel);
            if (modelBinder == null)
            {
                var error = new StringBuilder()
                    .AppendLine("Ошибка - Не возможно создать привязку!")

                    .Append("Controller: ")
                    .AppendLine(parameterModel.Action.Controller.ControllerName)
                    .Append("Method: ")
                    .AppendLine(parameterModel.Action.ActionName)
                    .Append("Parameter: ")
                    .AppendLine(parameterModel.ParameterName)
                    .ToString();

                throw new InvalidOperationException(error);
            }

            //-- Сохраняем!
            parameterModel.ModelBinder = modelBinder;
        }


        #endregion


        private static void AddRange<T>(IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
