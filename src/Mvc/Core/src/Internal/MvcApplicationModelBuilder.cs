using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.Internal
{

    public static class MvcApplicationModelBuilder
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
            controllerModel.Name = name;
            controllerModel.Index = displayInfo?.Index ?? controllerModel.Name.ToIntFromEnd();
            controllerModel.Title = displayInfo?.Title;                                           //TODO: add unti test

            return controllerModel;
        }

        public static BackgroundModel CreateBackgroundModel(TypeInfo backgroundType)
        {
            if (backgroundType == null)
            {
                return null;
            }

            //--
            var attributes = backgroundType.GetCustomAttributes(inherit: true).ToArray();

            //-- Конфигурация DisplayInfo
            var displayInfo = DisplayInfoBuilder.GetDisplayInfo(attributes);

            var name = backgroundType.Name.EndsWith("Model", StringComparison.OrdinalIgnoreCase)
                     ? backgroundType.Name.Substring(0, backgroundType.Name.Length - "Model".Length)
                     : backgroundType.Name;

            //--
            var backgroundModel = new BackgroundModel(backgroundType, attributes);
            backgroundModel.Name = name;
            backgroundModel.Number = displayInfo?.Number;
            backgroundModel.Title = displayInfo?.Title;

            return backgroundModel;
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
                if (bindingInfo.ModelPath == null) bindingInfo.ModelPath = propertyName;
                if (bindingInfo.ModelType == null) bindingInfo.ModelType = propertyInfo.PropertyType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }
            else
                return null;

            //--
            var propertyModel = new PropertyModel(propertyInfo, attributes);

            propertyModel.BindingInfo = bindingInfo;
            propertyModel.Name = propertyInfo.Name;

            return propertyModel;
        }

        //TODO: Написать юнит тесты
        public static MethodModel CreateMethodModel(MethodInfo methodInfo)
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
            var methodModel = new MethodModel(methodInfo, attributes);

            methodModel.BindingInfo = bindingInfo;
            methodModel.Name = methodInfo.Name;
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
                if (bindingInfo.ModelPath == null) bindingInfo.ModelPath = parameterName;
                if (bindingInfo.ModelType == null) bindingInfo.ModelType = parameterInfo.ParameterType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }
            else
            {
                bindingInfo = new BindingInfo();
                bindingInfo.ModelPath = parameterInfo.Name;
                bindingInfo.ModelType = parameterInfo.ParameterType;
                bindingInfo.BindingSource = BindingSource.DataContext;
            }

            //--
            var parameterModel = new ParameterModel(parameterInfo, attributes);
            //--
            parameterModel.BindingInfo = bindingInfo;
            parameterModel.Name = parameterInfo.Name;
            //--
            return parameterModel;
        }
    }
}
