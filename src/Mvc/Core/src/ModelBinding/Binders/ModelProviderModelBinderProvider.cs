using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Concurrent;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    /// <summary>
    /// 
    /// </summary> 
    public delegate IModelProvider ModelProviderFactoryDelagate();

    /// <summary>
    /// Провайдер привязки для источника <see cref="BindingSource.ModelProvider"/>
    /// </summary>   
    public class ModelProviderModelBinderProvider : IBinderProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, ModelProviderFactoryDelagate> modelProviderFactoryCache = new ConcurrentDictionary<Type, ModelProviderFactoryDelagate>();

        public ModelProviderModelBinderProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IModelBinder GetBinder(BinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.BindingInfo.BindingSource != null &&
                context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.ModelProvider) &&
                context.BindingInfo.ModelType != null)
            {
                var modelType = context.BindingInfo.ModelType;
                var modelProviderFactory = modelProviderFactoryCache.GetOrAdd(modelType, CreateFactory);
                return new ModelProviderModelBinder(modelProviderFactory);
            }

            return null;
        }

        private ModelProviderFactoryDelagate CreateFactory(Type modelType)
        {
            var modelProviderType = typeof(IModelProvider<>).MakeGenericType(modelType);

            return Factory;

            IModelProvider Factory()
            {
                var service = _serviceProvider.GetService(modelProviderType);

                if (service == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Не удается получить сервис типа {0}.. Возможно он не зарегистрирован",
                        modelProviderType));
                }

                return service as IModelProvider;
            }
        }
    }
}
