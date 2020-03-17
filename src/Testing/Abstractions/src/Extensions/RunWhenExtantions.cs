using System;

namespace EquipApps.Testing
{
    using Predicate = Func<TestContext, bool>;

    // <summary>
    /// Методы расширения, конфигурация конвеера
    /// </summary
    public static class RunWhenExtantions
    {
        // <summary>
        /// Модифицирует <see cref="TestDelegate"/>.
        /// Используется для разветвления конвеера
        /// </summary>  
        public static ITestBuilder RunWhen(this ITestBuilder builder, Predicate predicate, TestDelegate middlewareBranch)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (middlewareBranch == null)
            {
                throw new ArgumentNullException(nameof(middlewareBranch));
            }

            return builder.Use(main =>
            {
                return context =>
                {
                    if (predicate(context))
                    {
                        return middlewareBranch(context);
                    }
                    else
                    {
                        return main(context);
                    }
                };
            });
        }
    }
}
