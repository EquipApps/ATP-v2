using EquipApps.Mvc.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Testing
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public static void UseMvc(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var mvcMiddleware = application.ApplicationServices.GetService<RuntimeMiddleware>();
            if (mvcMiddleware == null)
                throw new Exception(nameof(RuntimeMiddleware));

            application.Use(mvcMiddleware.RunAsync);
        }
    }
}
