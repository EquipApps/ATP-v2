using EquipApps.Mvc.Abstractions;
using EquipApps.Testing.Features;
using Microsoft.Extensions.Options;
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
        IActionDescriptorCollectionProvider          _collectionProvider;
        private readonly IList<IMvcFeatureConvetion> _conventions;

        public MvcFeatureProvider(
            IActionDescriptorCollectionProvider collectionProvider, IOptions<MvcOptions> options)
        {
            if (collectionProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionProvider));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _collectionProvider = collectionProvider;
            _conventions = options.Value.FeatureConvetions;
        }

        public int Order => 0;

        public void OnProvidersExecuting(FeatureProviderContext context)
        {
            //-- 1) Создаем фичу
            var feature = new MvcFeature();

            //-- 2) Конфигурируем фичу
            feature.ActionDescriptors = GetActionDescriptors();

            foreach (var convention in _conventions)
            {
                convention.Apply(feature);
            }

            //-- 3) Сохраняем
            context.Collection.Set<IMvcFeature>(feature);
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
