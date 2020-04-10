using EquipApps.Mvc.Abstractions;
using EquipApps.Testing.Features;
using NLib.AtpNetCore.Testing.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Создает и сохраняет <see cref="MvcFeature"/>
    /// </summary>
    public class MvcFeatureProvider : IFeatureProvider
    {
        IActionDescriptorCollectionProvider _collectionProvider;

        public MvcFeatureProvider(IActionDescriptorCollectionProvider collectionProvider)
        {
            _collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
        }

        public int Order => 0;

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //-- Извлекаем из колллекции
            var actionDescriptorFeature = context.Collection.Get<IMvcFeature>();
            if (actionDescriptorFeature == null)
            {
                //-- Если null (первый запуск).. добавляем!
                actionDescriptorFeature = new MvcFeature();
                actionDescriptorFeature.ActionDescriptors = GetActionDescriptors();
                context.Collection.Set(actionDescriptorFeature);
            }
            else
            {
                //-- Обновляем коллекцию!
                actionDescriptorFeature.ActionDescriptors = null;
                actionDescriptorFeature.ActionDescriptors = GetActionDescriptors();
            }
        }

        private IReadOnlyList<ActionDescriptor> GetActionDescriptors()
        {
            return _collectionProvider.ActionDescriptors.Items.OrderBy(x => x.Number).ToArray();
        }

        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            //-- Ничегоне делаем
        }
    }
}
