using EquipApps.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    /// <summary>
    /// Создает <see cref="IActionInvoker"/> используя <see cref="IActionInvokerProvider"/>
    /// </summary>
    public class RuntimeActionInvokerFactory : IActionInvokerFactory, IDisposable
    {
        IActionInvokerProvider[] _providers;

        public RuntimeActionInvokerFactory(IEnumerable<IActionInvokerProvider> actionInvokerProviders)
        {
            _providers = actionInvokerProviders
                .OrderBy(x => x.Order)
                .ToArray();

            if (_providers.Length == 0)
            {
                throw new ArgumentException(nameof(_providers));
            }
        }

        public IActionInvoker CreateInvoker(ActionContext actionContext)
        {
            var context = new ActionInvokerProviderContext(actionContext);

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnProvideExecuted(context);
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
