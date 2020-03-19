using EquipApps.Testing;
using EquipApps.Testing.Features;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public static class TestContextEx
    {
        public static IReadOnlyList<ActionDescriptor> GetActionDescriptors(this TestContext testContext)
        {
            //-- 1) Извлечение
            return testContext.TestFeatures.GetActionDescriptors();
        }

        public static IReadOnlyList<ActionDescriptor> GetActionDescriptors(this IFeatureCollection Features)
        {
            //-- 1) Извлечение
            var actionDescriptorFeature = Features.Get<IMvcFeature>();
            if (actionDescriptorFeature == null)
            {
                throw new InvalidOperationException(
                   string.Format("{0} не содержит {1}", nameof(TestContext), nameof(IMvcFeature)));
            }

            var actionDescriptors = actionDescriptorFeature.ActionDescriptors;
            if (actionDescriptors == null || actionDescriptors.Count == 0)
            {
                throw new InvalidOperationException(
                    string.Format("{0} не содержит {1}", nameof(IMvcFeature), nameof(ActionDescriptor)));
            }

            return actionDescriptors;
        }
    }
}
