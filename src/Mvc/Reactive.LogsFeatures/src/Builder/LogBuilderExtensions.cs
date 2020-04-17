using EquipApps.Mvc.Reactive.LogsFeatures.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Testing
{
    public static class LogBuilderExtensions
    {
        /// <summary>
        /// Очистка журнала сообщений
        /// </summary>
        public static void UseLogsClean(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var middleware = application.ApplicationServices.GetService<LogsMiddleware>();
            if (middleware == null)
                throw new Exception(nameof(middleware));

            application.Use(middleware.RunCleanAsync);
        }
    }
}
