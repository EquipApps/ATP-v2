using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.ApplicationModels
{
    public static class ControllerActionDescriptorBuilder
    {
        public static ControllerActionDescriptor Flattener(ApplicationModel application, ControllerModel controller, ActionModel action,
                                                           ControllerTestCase testCase, ControllerTestStep testStep)
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
