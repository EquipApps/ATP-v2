using Microsoft.Extensions.Internal;

namespace EquipApps.Mvc.Infrastructure
{
    public class ControllerActionInvokerCacheEntry
    {
        public ControllerActionInvokerCacheEntry(
            ObjectMethodExecutor objectMethodExecutor,
            ActionMethodExecutor actionMethodExecutor)
        {
            ObjectMethodExecutor = objectMethodExecutor;

            ActionMethodExecutor = actionMethodExecutor;
        }


        public ObjectMethodExecutor ObjectMethodExecutor { get; }

        public ActionMethodExecutor ActionMethodExecutor { get; }
    }
}
