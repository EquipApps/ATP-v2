using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Расширения для <see cref="TestOptions"/>
    /// </summary>
    public static class TestOptionsExtantions
    {
        public static T GetProperty<T>(this TestOptions options)
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

        public static T GetProperty<T>(this TestOptions options, string key)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }


            object value;

            if (options.Properties.TryGetValue(key, out value))
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }


        public static void SetProperty<T>(this TestOptions options, T value)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Properties[typeof(T)] = value;
        }

        public static void SetProperty<T>(this TestOptions options, T value, string key)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Properties[key] = value;
        }

    }
}
