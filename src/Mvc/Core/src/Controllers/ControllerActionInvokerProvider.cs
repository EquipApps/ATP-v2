using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Controllers;
using EquipApps.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace EquipApps.Mvc.Infrastructure
{
    internal class ControllerActionInvokerProvider : IActionInvokerProvider
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;

        private ControllerActionInvokerCache _controllerActionInvokerCache;
        private ControllerCache _controllerCache;

        private WeakReference<ControllerTestCase> weak_lastTestCase = new WeakReference<ControllerTestCase>(null);

        public ControllerActionInvokerProvider(
            ControllerActionInvokerCache controllerActionInvokerCache,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _controllerActionInvokerCache = controllerActionInvokerCache ?? throw new ArgumentNullException(nameof(controllerActionInvokerCache));

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger<ControllerActionInvoker>();

            
        }

        private ControlleFactoryDelegate controllerFactory
        {
            get
            {
                var current = _controllerCache;
                if (current == null)
                {
                    current = new ControllerCache(_serviceProvider);
                    _controllerCache = current;
                }

                return current.GetCachedResult;
            }
        }

        public int Order => 0;
        
        public void OnDisposeExecuted()
        {
            _controllerCache = null;
        }

        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            if (context.ActionContext.ActionObject.ActionDescriptor is ControllerActionDescriptor)
            {
                //-- 
                var controllerContext = new ControllerContext(context.ActionContext);

                //--
                var controllerCacheEntry = _controllerActionInvokerCache.GetCachedResult(controllerContext);

                //--
                var invoker = new ControllerActionInvoker(controllerContext,
                                                          controllerCacheEntry,
                                                          controllerFactory,
                                                          _logger);
                context.Result = invoker;
            }
        }

        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
            //-- Ничего не делаем.
        }
    }
}
