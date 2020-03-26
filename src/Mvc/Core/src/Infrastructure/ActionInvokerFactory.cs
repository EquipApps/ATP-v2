using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// Создает <see cref="IActionInvoker"/> используя <see cref="IActionInvokerProvider"/>
    /// </summary>
    public class ActionInvokerFactory : IActionInvokerFactory
    {
        IActionInvokerProvider[] _providers;

        public ActionInvokerFactory(IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            _providers = actionInvokerProviders.OrderBy(x => x.Order).ToArray();
        }

        public IActionInvoker CreateInvoker(ActionContext actionContext)
        {
            var context = new ActionInvokerProviderContext(actionContext);

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnProvidersExecuting(context);
            }

            for (var i = _providers.Length - 1; i >= 0; i--)
            {
                _providers[i].OnProvidersExecuted(context);
            }

            return context.Result;
        }

        public void Dispose()
        {
            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnDisposeExecuted();
            }
        }
    }
}
