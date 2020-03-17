using EquipApps.Hardware;
using EquipApps.Testing.Features;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HardwareServiceCollectionExtensions
    {
        public static void AddHardware(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }


            //--
            services.AddSingleton<HardwareMiddleware>();
            services.AddSingleton<IHardwareCollection, HardwareCollection>();

            //--
            services.AddTransient<IFeatureProvider, HardwareFeatureProvider>();     //-- Enumerable
            services.AddTransient<IHardwareFeature, HardwareFeature>();
        }
    }
}
