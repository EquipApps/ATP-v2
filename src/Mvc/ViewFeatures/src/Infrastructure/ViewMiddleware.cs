﻿using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Services;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Главный промежуточный слой.
    /// Обновляет состояние
    /// </summary>
    /// 
    public class ViewMiddleware
    {
        private IActionService _actionService;

        public ViewMiddleware(IActionService actionService)
        {
            _actionService = actionService ?? throw new ArgumentNullException(nameof(actionService));
        }

        public Task RunAsync(TestContext testContext)
        {
            return Task.Run(() => Run(testContext));
        }

        private void Run(TestContext testContext)
        {
            //-- 1) Извлечение дескриптеров действий
            var actionDescriptors = testContext.GetActionDescriptors();

            //-- 2) Обновляем состояние.
            foreach (var actionDescriptor in actionDescriptors)
            {
                actionDescriptor.SetResult(ActionDescriptorResultType.NotRun);
            }

            _actionService.Update(actionDescriptors);
        }
    }
}
