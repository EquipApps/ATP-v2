using System;

namespace EquipApps.Hardware
{
    public static class RegisterHardwareExtentions
    {
        public static void RegisterHardware<TBehavior>(this HardwareOptions options, string hardwareName)
           where TBehavior : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior));
        }
        public static void RegisterHardware<TBehavior1, TBehavior2>(this HardwareOptions options, string hardwareName)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior1), typeof(TBehavior2));
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3>(this HardwareOptions options, string hardwareName)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3));
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4>(this HardwareOptions options, string hardwareName)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4));
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4, TBehavior5>(this HardwareOptions options, string hardwareName)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior
            where TBehavior5 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4), typeof(TBehavior5));
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4, TBehavior5, TBehavior6>(this HardwareOptions options, string hardwareName)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior
            where TBehavior5 : class, IHardwareBehavior
            where TBehavior6 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.RegisterHardware(hardwareName, typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4), typeof(TBehavior5), typeof(TBehavior6));
        }


        public static void RegisterHardware(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)           
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[0];
           

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
           where TBehavior1 : class, IHardwareBehavior          
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1)
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1, TBehavior2>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior           
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1), typeof(TBehavior2),
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior          
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3),
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior        
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4),
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4, TBehavior5>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior
            where TBehavior5 : class, IHardwareBehavior
         
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4), typeof(TBehavior5),
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }
        public static void RegisterHardware<TBehavior1, TBehavior2, TBehavior3, TBehavior4, TBehavior5, TBehavior6>(this HardwareOptions options, string hardwareNameFormat, int count, int startNumber = 0)
            where TBehavior1 : class, IHardwareBehavior
            where TBehavior2 : class, IHardwareBehavior
            where TBehavior3 : class, IHardwareBehavior
            where TBehavior4 : class, IHardwareBehavior
            where TBehavior5 : class, IHardwareBehavior
            where TBehavior6 : class, IHardwareBehavior
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(hardwareNameFormat))
                throw new ArgumentNullException(nameof(hardwareNameFormat));

            //--
            var endNumber     = startNumber + count;
            var behaviorTypes = new Type[]
            {
                typeof(TBehavior1), typeof(TBehavior2), typeof(TBehavior3), typeof(TBehavior4), typeof(TBehavior5), typeof(TBehavior6),
            };

            //--
            for (int number = startNumber; number < endNumber; number++)
            {
                string hardwareName = string.Format(hardwareNameFormat, number);

                options.RegisterHardware(hardwareName, behaviorTypes);
            }
        }



        public static void RegisterMapping<TDevice, TAdapter>(this HardwareOptions options, string mapName)
            where TDevice : class
            where TAdapter : class, IHardwareAdapter

        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.RegisterMapping(typeof(TAdapter), typeof(TDevice), mapName, null);
        }

        public static void RegisterMapping<TDevice, TAdapter>(this HardwareOptions options, string mapName, Func<TDevice> factory)
            where TDevice : class
            where TAdapter : class, IHardwareAdapter

        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.RegisterMapping(typeof(TAdapter), typeof(TDevice), mapName, factory);
        }


    }
}
