using Microsoft.Extensions.Internal;

namespace EquipApps.Mvc.Infrastructure
{
    internal class ControllerActionInvokerCacheEntry
    {
        public ControllerActionInvokerCacheEntry(
            ObjectMethodExecutor objectMethodExecutor,
            ActionMethodExecutor actionMethodExecutor)
        {
            ObjectMethodExecutor = objectMethodExecutor;

            ActionMethodExecutor = actionMethodExecutor;
        }


        internal ObjectMethodExecutor ObjectMethodExecutor { get; }

        internal ActionMethodExecutor ActionMethodExecutor { get; }
    }
}
