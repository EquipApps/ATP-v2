using EquipApps.Hardware;
using EquipApps.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Builder
{
    public static class HardwareBuilderExtantions
    {
        public static ITestBuilder UseHardware(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var hardwareMiddleware = application.ApplicationServices.GetService<HardwareMiddleware>();
            if (hardwareMiddleware == null)
                throw new Exception(nameof(HardwareMiddleware));

            return application.Use(hardwareMiddleware.Add);

        }
    }
}
