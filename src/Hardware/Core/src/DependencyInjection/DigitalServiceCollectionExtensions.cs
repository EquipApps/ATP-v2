using EquipApps.Hardware;
using EquipApps.Testing.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DigitalServiceCollectionExtensions
    {
        public static void AddHardwareDigital(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IFeatureProvider, DigitalFeatureProvider>();     //-- Enumerable
        }
    }
}
