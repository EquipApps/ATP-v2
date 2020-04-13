﻿using EquipApps.Mvc.Infrastructure;
using EquipApps.Mvc.Services;
using EquipApps.Mvc.Viewers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ViewerServiceCollectionExtensions 
    {
        public static void AddViewer(this IServiceCollection services)
        {
            //-- Extentions
            services.AddTransientFeatureProvider<ViewFeatureProvider>();
            services.AddTransientActionInvokerProvider<ViewActionInvokerProvider>();

            //-- Регистрация сервисов
            services.AddSingleton<IActionService, ActionService>();
            services.AddSingleton<ILogService, LogService>();


            //-- Viewers
            services.AddTransient<ActionsViewer>();
            services.AddTransient<LogViewer>();

        }
    }
}
