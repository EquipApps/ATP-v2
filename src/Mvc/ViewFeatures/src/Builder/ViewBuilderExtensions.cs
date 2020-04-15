using EquipApps.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Testing
{
    public static class ViewBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public static void UseView(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var middleware = application.ApplicationServices.GetService<ViewMiddleware>();
            if (middleware == null)
                throw new Exception(nameof(ViewMiddleware));

            application.Use(middleware.RunAsync);
        }
    }
}
