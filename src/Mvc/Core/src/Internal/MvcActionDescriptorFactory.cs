using EquipApps.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.Internal
{
    public class MvcActionDescriptorFactory : IActionFactory
    {
        IActionProvider[] _providers;

        public MvcActionDescriptorFactory(
            IEnumerable<IActionProvider> actionDescriptorProviders)
        {
            if (actionDescriptorProviders == null)
                throw new ArgumentNullException(nameof(actionDescriptorProviders));

            _providers = actionDescriptorProviders.ToArray();

            if (_providers.Length == 0)
            {
                throw new ArgumentException(nameof(_providers));
            }
        }

        public IReadOnlyList<ActionDescriptor> GetActionDescriptors()
        {
            var context = new ActionDescriptorContext();

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].OnProvidersExecuting(context);
            }

            for (var i = _providers.Length - 1; i >= 0; i--)
            {
                _providers[i].OnProvidersExecuted(context);
            }

            return context.Results;
        }
    }
}
