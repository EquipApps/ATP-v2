using EquipApps.Mvc;
using Microsoft.Extensions.Logging;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerActionInvokerProvider : IActionInvokerProvider
    {
        private ILogger<ControllerActionInvoker> logger;
        private ControllerCache controllerCache;
        private ControllerActionInvokerCache controllerActionInvokerCache;

        public ControllerActionInvokerProvider(ILoggerFactory loggerFactory, IControllerFactory controllerFactory)
        {
            this.logger = loggerFactory.CreateLogger<ControllerActionInvoker>();

            controllerCache             = new ControllerCache(controllerFactory);
            controllerActionInvokerCache = new ControllerActionInvokerCache();
        }

        public int Order => 0;

        public void OnDisposeExecuted()
        {
            controllerCache.Clear();
        }

        public void OnProvideExecuted(ActionInvokerProviderContext context)
        {
            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor)
            {
                var controllerContext = new ControllerContext(context.ActionContext);
                var controllerCacheEntry = controllerActionInvokerCache.GetCachedResult(controllerContext);

                context.Result = new ControllerActionInvoker(
                    controllerContext,
                    controllerCacheEntry,
                    controllerCache.GetCachedResult,
                    logger);
            }
        }







    }
}
