using EquipApps.Mvc.Abstractions;
using EquipApps.Testing.Features;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Создает <see cref="IMvcFeature"/> и сохранаяте в <see cref="IFeatureCollection"/>.
    /// </summary>
    public class MvcFeatureProvider : IFeatureProvider
    {
        IActionDescriptorCollectionProvider   _collectionProvider;
        private readonly IMvcFeatureConvetion _convention;

        public MvcFeatureProvider(
            IActionDescriptorCollectionProvider collectionProvider,
            IMvcFeatureConvetion mvcFeatureConvetion = null)
        {
            if (collectionProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionProvider));
            }

            _collectionProvider = collectionProvider;
            _convention = mvcFeatureConvetion;
           
        }

        public int Order => 0;

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //-- 1) Создаем фичу
            var feature = new MvcFeature();

            //-- 2) Конфигурируем фичу
            feature.ActionObjects = GetActionDescriptors();

            _convention?.Apply(feature);

            //-- 3) Сохраняем
            context.Collection.Set<IMvcFeature>(feature);
        }

        private IReadOnlyList<ActionObject> GetActionDescriptors()
        {
            /*
             * 1) Извлеает дескриптеры рействий.
             * 2) Сортирует
             * 3) Сбрасывает в исходное состояние.
             */

            return _collectionProvider.ActionDescriptors.Items
                .OrderBy(x => x.Number)
                .Select(x => new ActionObject(x))
                .ToArray();
        }

        public void OnProvidersExecuted(FeatureProviderContext context)
        {
            //-- Ничегоне делаем
        }
    }
}
