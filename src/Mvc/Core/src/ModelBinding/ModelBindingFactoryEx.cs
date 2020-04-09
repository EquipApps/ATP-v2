using System;
using System.Diagnostics;

namespace EquipApps.Mvc.ModelBinding
{
    internal static class ModelBindingFactoryEx
    {
        public static IModelBinder Create(this IModelBindingFactory modelBindingFactory, IBindingModel bindingModel)
        {
            Debug.Assert(modelBindingFactory != null);
            Debug.Assert(bindingModel != null);

            //TODO: Реалезовать Unit Test.. Возвращает нулевую привязку если BindingInfo == null
            if (bindingModel.BindingInfo == null)
            {
                return null;
            }

            return modelBindingFactory.Create(bindingModel, bindingModel.BindingInfo);
        }
        public static IModelBinder Create(this IModelBindingFactory modelBindingFactory, IBindingModel bindingModel, string format)
        {
            Debug.Assert(modelBindingFactory != null);
            Debug.Assert(bindingModel != null);

            //TODO: Реалезовать Unit Test.. Возвращает нулевую привязку если format == null
            if (format == null)
            {
                return null;
            }

            var bindingInfo = new BindingInfo()
            {
                BindingSource = BindingSource.DataText,
                BindingModelName = format
            };

            return modelBindingFactory.Create(bindingModel, bindingInfo);
        }
    }
}
