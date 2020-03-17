using System;

namespace EquipApps.Testing
{
    using Predicate = Func<TestContext, bool>;

    // <summary>
    /// Методы расширения, конфигурация конвеера
    /// </summary
    public static class UseWhenExtensions
    {
        // <summary>
        /// Модифицирует <see cref="TestDelegate"/>.
        /// Используется для разветвления конвеера
        /// </summary>  
        public static ITestBuilder UseWhen(this ITestBuilder builder, Predicate predicate, Action<ITestBuilder> configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Create and configure the branch builder right away; otherwise,
            // we would end up running our branch after all the components
            // that were subsequently added to the main builder.
            var branchBuilder = builder.New();
            configuration(branchBuilder);

            return builder.Use(main =>
            {
                // This is called only when the main application builder 
                // is built, not per request.
                //branchBuilder.Run(main);
                var branch = branchBuilder.Build();

                return context =>
                {
                    if (predicate(context))
                    {
                        return branch(context);
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
