using EquipApps.Mvc.Internal;
using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public static class ApplicationModelBuilder
    {
        public static ControllerModel CreateControllerModel(TypeInfo controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            //--
            var attributes = controllerType.GetCustomAttributes(inherit: true).ToArray();

            //-- Конфигурация BindingInfo
            var bindingInfo = BindingInfoBuilder.GetBindingInfo(attributes);
            if (bindingInfo == null)
            {
                //-- Проверяем наличие метки IModelExpected<>
                var modelExpectedType = controllerType.ImplementedInterfaces
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IModelExpected<>));

                if (modelExpectedType != null)
                {
                    bindingInfo = new BindingInfo();
                    bindingInfo.BindingModelType = modelExpectedType.GenericTypeArguments.FirstOrDefault();
                    bindingInfo.BindingSource = BindingSource.ModelProvider;
                }
            }

            //-- Конфигурация DisplayInfo
            var displayInfo = DisplayInfoBuilder.GetDisplayInfo(attributes);

            var name = controllerType.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                     ? controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length)
                     : controllerType.Name;

            //--
            var controllerModel = new ControllerModel(controllerType, attributes);
            //controllerModel.Area = displayInfo?.Area;
            controllerModel.BindingInfo = bindingInfo;
            controllerModel.ControllerName = name;
            //controllerModel.Index = displayInfo?.Index ?? controllerModel.ControllerName.ToIntFromEnd();
            controllerModel.Title = displayInfo?.Title;                                           //TODO: add unti test

            return controllerModel;
        }

        

        //TODO: Написать юнит тесты
        public static ActionModel CreateMethodModel(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            //--
            var attributes = methodInfo.GetCustomAttributes(inherit: true)
                .ToArray();
            //--          
            var bindingInfo = BindingInfoBuilder.GetBindingInfo(attributes);
            //-- 
            var displayInfo = DisplayInfoBuilder.GetDisplayInfo(attributes);

            //--
            var methodModel = new ActionModel(methodInfo, attributes);

            methodModel.BindingInfo = bindingInfo;
            methodModel.ActionName = methodInfo.Name;
            methodModel.Number = displayInfo?.Number;
            methodModel.Title = displayInfo?.Title;

            return methodModel;
        }

       
    }
}
