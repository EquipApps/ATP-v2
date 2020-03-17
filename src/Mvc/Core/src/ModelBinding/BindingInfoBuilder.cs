using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Билдер <see cref="BindingInfo"/>
    /// </summary>
    public static class BindingInfoBuilder
    {
        /// <summary>
        /// Извлекакт информацию о привязке из аттрибутов
        /// </summary>
        /// 
        /// <param name="attributes">
        /// Аттрибуты
        /// </param>
        /// 
        /// <returns>
        /// Возвращает <see cref="BindingInfo"/> если есть привязка, или NULL если привязки нет!
        /// </returns>
        public static BindingInfo GetBindingInfo(IEnumerable<object> attributes)
        {
            var bindingInfo = new BindingInfo();
            var isBindingInfoPresent = false;

            // IBindingSourceMetadata
            foreach (var bindingSourceAttribute in attributes.OfType<IBindingSourceMetadata>())
            {
                isBindingInfoPresent = true;
                if (bindingSourceAttribute.BindingSource != null)
                {
                    bindingInfo.BindingSource = bindingSourceAttribute.BindingSource;
                    break;
                }
            }

            // IBinderTypeMetadata
            foreach (var binderTypeAttribute in attributes.OfType<IBinderTypeMetadata>())
            {
                isBindingInfoPresent = true;
                if (binderTypeAttribute.BinderType != null)
                {
                    bindingInfo.BinderType = binderTypeAttribute.BinderType;
                    break;
                }
            }

            // IModelNameMetadata
            foreach (var binderModelNameAttribute in attributes.OfType<IModelPathMetadata>())
            {
                isBindingInfoPresent = true;
                if (binderModelNameAttribute?.ModelPath != null)
                {
                    bindingInfo.ModelPath = binderModelNameAttribute.ModelPath;
                    break;
                }
            }

            // IModelTypeMetadata
            foreach (var binderModelTypeAttribute in attributes.OfType<IModelTypeMetadata>())
            {
                isBindingInfoPresent = true;
                if (binderModelTypeAttribute?.ModelType != null)
                {
                    bindingInfo.ModelType = binderModelTypeAttribute.ModelType;
                    break;
                }
            }


            return isBindingInfoPresent ? bindingInfo : null;
        }
    }
}