using EquipApps.Testing;
using EquipApps.Testing.Features;
using System;

namespace EquipApps.Hardware
{
    public static class TestContextExtantoions
    {
        public static IHardwareFeature GetHardwareFeature(this TestContext testContext)
        {
            //-- 1) Извлечение
            return testContext.TestFeatures.GetHardwareFeature();
        }

        public static IHardwareFeature GetHardwareFeature(this IFeatureCollection Features)
        {
            //-- 1) Извлечение
            var hardwareFeature = Features.Get<IHardwareFeature>();
            if (hardwareFeature == null)
            {
                throw new InvalidOperationException(
                   string.Format("{0} не содержит {1}", nameof(TestContext), nameof(IHardwareFeature)));
            }

            return hardwareFeature;
        }
    }
}
