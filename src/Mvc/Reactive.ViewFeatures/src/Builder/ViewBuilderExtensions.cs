using EquipApps.Mvc;
using EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EquipApps.Testing
{
    public static class ViewBuilderExtensions
    {
        /// <summary>
        /// Обновляет состояние <see cref="ActionObject"/>
        /// </summary>
        public static void UseViewUpdate(this ITestBuilder application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var middleware = application.ApplicationServices.GetService<ViewMiddleware>();
            if (middleware == null)
                throw new Exception(nameof(ViewMiddleware));

            application.Use(middleware.RunUpdateAsync);
        }
    }
}
