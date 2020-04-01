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
                    bindingInfo.ModelType = modelExpectedType.GenericTypeArguments.FirstOrDefault();
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
            controllerModel.Area = displayInfo?.Area;
            controllerModel.BindingInfo = bindingInfo;
            controllerModel.ControllerName = name;
            controllerModel.Index = displayInfo?.Index ?? controllerModel.ControllerName.ToIntFromEnd();
            controllerModel.Title = displayInfo?.Title;                                           //TODO: add unti test

            return controllerModel;
        }

        //TODO: Написать юнит тесты
        public static PropertyModel CreatePropertyModel(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            //--
            var attributes = propertyInfo.GetCustomAttributes(inherit: true).ToArray();
            //--
            var propertyName = propertyInfo.Name;

            /* 
             * Ищем привязку 
             * Выбираем только те свойства
             * для который установленна привязка! 
             */
            var bindingInfo = BindingInfoBuilder.GetBindingInfo(attributes);
            if (bindingInfo != null)
            {
                /*
                 * Если Имя модели NULL      => Привязка по имени свойства
                 * Если Тип модели NULL      => Привязка по типу свойства
                 * Если Источник модели NULL => Привязка из DataContext
                 */
                if (bindingInfo.BinderModelName == null) bindingInfo.BinderModelName = propertyName;
                if (bindingInfo.ModelType == null) bindingInfo.ModelType = propertyInfo.PropertyType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }
            else
                return null;

            //--
            var propertyModel = new PropertyModel(propertyInfo, attributes);

            propertyModel.BindingInfo = bindingInfo;
            propertyModel.PropertyName = propertyInfo.Name;

            return propertyModel;
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

        //TODO: Написать юнит тесты
        public static ParameterModel CreateParameterModel(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            //--
            var attributes = parameterInfo.GetCustomAttributes(inherit: true).ToArray();

            //-- ParameterName
            var parameterName = parameterInfo.Name;

            //--- Тут можно добавить логику обработки атрибутов !! 
            var bindingInfo = BindingInfoBuilder.GetBindingInfo(attributes);
            if (bindingInfo != null)
            {
                /*
                * Если Имя модели NULL      => Привязка по имени параметра
                * Если Тип модели NULL      => Привязка по типу параметра
                * Если Источник модели NULL => Привязка из DataContext
                */
                if (bindingInfo.BinderModelName == null) bindingInfo.BinderModelName = parameterName;
                if (bindingInfo.ModelType == null) bindingInfo.ModelType = parameterInfo.ParameterType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }
            else
            {
                bindingInfo = new BindingInfo();
                bindingInfo.BinderModelName = parameterInfo.Name;
                bindingInfo.ModelType = parameterInfo.ParameterType;
                bindingInfo.BindingSource = BindingSource.DataContext;
            }

            //--
            var parameterModel = new ParameterModel(parameterInfo, attributes);
            //--
            parameterModel.BindingInfo = bindingInfo;
            parameterModel.ParameterName = parameterInfo.Name;
            //--
            return parameterModel;
        }
    }
}
