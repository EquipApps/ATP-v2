using EquipApps.Testing;
using EquipApps.Testing.Features;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public static class TestContextEx
    {
        public static IReadOnlyList<ActionObject> GetActionObjects(this TestContext testContext)
        {
            //-- 1) Извлечение
            return testContext.TestFeatures.GetActionObjects();
        }

        public static IReadOnlyList<ActionObject> GetActionObjects(this IFeatureCollection Features)
        {
            //-- 1) Извлечение
            var feature = Features.Get<IMvcFeature>();
            if (feature == null)
            {
                throw new InvalidOperationException(
                   string.Format("{0} не содержит {1}", nameof(TestContext), nameof(IMvcFeature)));
            }

            var actionObjects = feature.ActionObjects;
            if (actionObjects == null || actionObjects.Count == 0)
            {
                throw new InvalidOperationException(
                    string.Format("{0} не содержит {1}", nameof(IMvcFeature), nameof(ActionObject)));
            }

            return actionObjects;
        }
    }
}
