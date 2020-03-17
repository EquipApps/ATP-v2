namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public class ControllerActionInvokerCacheEntry
    {
        public ControllerActionInvokerCacheEntry(
            ObjectMethodExecutor objectMethodExecutor,
            ControllerMethodExecutor actionMethodExecutor)
        {
            ObjectMethodExecutor = objectMethodExecutor;

            ActionMethodExecutor = actionMethodExecutor;
        }


        public ObjectMethodExecutor ObjectMethodExecutor { get; }

        public ControllerMethodExecutor ActionMethodExecutor { get; }
    }
}
