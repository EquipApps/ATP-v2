using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Hardware
{
    public static class HardwareOptionsExtantions
    {
        public static IEnumerable<string> GetVirtualDevice(this HardwareOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return options.VirtualDefines.Select(x => x.Name);
        }


        public static T GetProperty<T>(this HardwareOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            object value;

            if (options.Properties.TryGetValue(typeof(T), out value))
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }

        public static void SetProperty<T>(this HardwareOptions options, T value)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Properties[typeof(T)] = value;
        }
    }
}
