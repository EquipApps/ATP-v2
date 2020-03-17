using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Методы расширения, конфигурация конвеера
    /// </summary>
    public static class UseExtensions
    {
        /// <summary>
        /// Объединяет <see cref="TestDelegate"/> в конвеер.      
        /// </summary>       
        public static ITestBuilder Use(this ITestBuilder builder, TestDelegate middleware)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.Use(next => async context =>
            {
                await middleware(context);
                await next(context);
            });
        }

        /// <summary>
        /// Объединяет <see cref="TestDelegate"/> в конвеер.      
        /// </summary>   
        public static ITestBuilder Use(this ITestBuilder builder, Action<TestContext> middleware)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }


            return builder.Use(next => async context =>
            {
                middleware(context);
                await next(context);
            });
        }
    }
}
