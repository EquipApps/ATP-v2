using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using System;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure
{
    public class ViewActionInvokerProvider : IActionInvokerProvider
    {
        private IActionService _actionService;

        public ViewActionInvokerProvider(IActionService actionService)
        {
            _actionService = actionService ?? throw new ArgumentNullException(nameof(actionService));
        }

        public int Order => -1000;



        public void OnDisposeExecuted()
        {
            //---
        }

        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            //---
        }

        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
            //---
            var wrappResult = new ViewActionInvoker(_actionService,
                                                    context.ActionContext,
                                                    context.Result);
            context.Result = wrappResult;
        }
    }
}
