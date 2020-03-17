using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Методы расширения
    /// </summary>
    public static class RunExtensions
    {
        /// <summary>
        /// Добавить последний элемент в конвеер!
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="middleware"></param>
        public static void Run(this ITestBuilder builder, TestDelegate middleware)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            builder.Use(_ => middleware);
        }
    }
}
