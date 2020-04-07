using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class BindingInfoApplicationModelProvider : IApplicationModelProvider
    {
        
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public BindingInfoApplicationModelProvider(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public int Order => -900;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            foreach (var controller in context.Result.Controllers)
            {
                FillBindingInfoFor(controller);

                foreach (var property in controller.ControllerProperties)
                {
                    FillBindingInfoFor(property);
                }

                foreach (var action in controller.Actions)
                {
                    FillBindingInfoFor(action);

                    foreach (var parameter in action.Parameters)
                    {
                        FillBindingInfoFor(parameter);
                    }
                }
            }
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {            
        }

        private void FillBindingInfoFor(ControllerModel controller)
        {
            //-- Создание BindingInfo черех атрибуты
            var bindingInfo = BindingInfo.GetBindingInfo(controller.Attributes);
            if (bindingInfo == null)
            {
                //-- Создание BindingInfo через атрибут IModelExpected<>
                var modelExpectedType = controller.ControllerType.ImplementedInterfaces
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IModelExpected<>));

                if (modelExpectedType != null)
                {
                    bindingInfo = new BindingInfo();
                    bindingInfo.BindingModelType = modelExpectedType.GenericTypeArguments.FirstOrDefault();
                    bindingInfo.BindingSource = BindingSource.ModelProvider;
                }
            }

            controller.BindingInfo = bindingInfo;
        }

        private void FillBindingInfoFor(PropertyModel property)
        {
            var propertyInfo = property.PropertyInfo;
            var attributes   = property.Attributes;

            // BindingInfo for properties can be either specified by decorating the property with binding specific attributes.
            // ModelMetadata also adds information from the property's type and any configured IBindingMetadataProvider.
            var modelMetadata = _modelMetadataProvider.GetMetadataForProperty(propertyInfo.DeclaringType, propertyInfo.Name);
            var bindingInfo   = BindingInfo.GetBindingInfo(attributes, modelMetadata);

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
                if (bindingInfo.BindingModelName == null) bindingInfo.BindingModelName = property.Name;
                if (bindingInfo.BindingModelType == null) bindingInfo.BindingModelType = property.ParameterType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }

            property.BindingInfo = bindingInfo;
        }

        private void FillBindingInfoFor(ActionModel action)
        {
            //--  
            action.BindingInfo = BindingInfo.GetBindingInfo(action.Attributes);
        }

        private void FillBindingInfoFor(ParameterModel parameter)
        {
            var parameterInfo = parameter.ParameterInfo;
            var attributes = parameter.Attributes;

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
                if (bindingInfo.BindingModelName == null) bindingInfo.BindingModelName = parameter.Name;
                if (bindingInfo.BindingModelType == null) bindingInfo.BindingModelType = parameter.ParameterType;
                if (bindingInfo.BindingSource == null) bindingInfo.BindingSource = BindingSource.DataContext;
            }
            
            //-- Все параметры должны иметь привязку.
            //-- Если привязка не создана, делаем поумолчанию
            if(bindingInfo == null)
            {
                bindingInfo = new BindingInfo();
                bindingInfo.BindingModelName = parameter.Name;
                bindingInfo.BindingModelType = parameter.ParameterType;
                bindingInfo.BindingSource = BindingSource.DataContext;
            }
        }
    }
}
