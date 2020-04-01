using EquipApps.Mvc.ModelBinding;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    public static class BindingFactoryEx
    {
        public static IModelBinder Create(this IBindingFactory factory, IBindingModel bindingModel)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (bindingModel == null)
            {
                throw new ArgumentNullException(nameof(bindingModel));
            }

            if (bindingModel.BindingInfo == null)
            {
                throw new ArgumentNullException(nameof(bindingModel.BindingInfo));
            }

            return factory.Create(bindingModel, bindingModel.BindingInfo);
        }
        public static IModelBinder Create(this IBindingFactory factory, IBindingModel bindingModel, string format)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (bindingModel == null)
            {
                throw new ArgumentNullException(nameof(bindingModel));
            }

            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            var bindingInfo = new BindingInfo()
            {
                BindingSource = BindingSource.DataText,
                BinderModelName = format
            };

            return factory.Create(bindingModel, bindingInfo);
        }
    }
}
