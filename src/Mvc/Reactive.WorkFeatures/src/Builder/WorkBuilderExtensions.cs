using EquipApps.Mvc.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Testing
{
    public static class WorkBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void UseRuntime(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var middleware = application.ApplicationServices.GetService<RuntimeMiddleware>();
            if (middleware == null)
                throw new Exception(nameof(RuntimeMiddleware));

            application.Use(middleware.RunAsync);
        }
    }
}
