using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Методы расширения, конфигурация конвеера
    /// </summary
    public static class UseUnhandledExceptionExtensions
    {
        /// <summary>
        /// Модифицирует <see cref="TestDelegate"/>.
        /// Используется для обработки исключений конвеера
        /// </summary>
        public static ITestBuilder UseUnhandledException(this ITestBuilder builder, Action<Exception> unhandledException)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (unhandledException == null)
            {
                throw new ArgumentNullException(nameof(unhandledException));
            }

            return builder.Use(main =>
            {
                return async context =>
                {
                    try
                    {
                        await main(context);
                    }
                    catch (Exception ex)
                    {
                        unhandledException(ex);
                    }
                };
            });
        }

        // <summary>
        /// Модифицирует <see cref="TestDelegate"/>.
        /// Используется для обработки исключений конвеера
        /// </summary>
        public static ITestBuilder UseUnhandledException(this ITestBuilder builder, Action<TestContext, Exception> unhandledException)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (unhandledException == null)
            {
                throw new ArgumentNullException(nameof(unhandledException));
            }

            return builder.Use(main =>
            {
                return async context =>
                {
                    try
                    {
                        await main(context);
                    }
                    catch (Exception ex)
                    {
                        unhandledException(context, ex);
                    }
                };
            });
        }
    }
}
