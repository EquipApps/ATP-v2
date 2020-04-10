using System;
using System.Diagnostics;

namespace EquipApps.Mvc.ModelBinding
{
    internal static class ModelBinderFactoryEx
    {
        public static IModelBinder Create(this IModelBinderFactory modelBindingFactory, IBindingModel bindingModel)
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
        public static IModelBinder Create(this IModelBinderFactory modelBindingFactory, IBindingModel bindingModel, string format)
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
                BindingModelPath = format
            };

            return modelBindingFactory.Create(bindingModel, bindingInfo);
        }
    }
}
