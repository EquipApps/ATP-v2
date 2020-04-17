using EquipApps.Mvc.Services;

namespace EquipApps.Mvc.Infrastructure
{
    public class ViewActionInvoker : IActionInvoker
    {
        private readonly IActionService actionService;
        private ActionContext actionContext;
        private IActionInvoker innerInvoker;
        

        public ViewActionInvoker(IActionService actionService, ActionContext actionContext, IActionInvoker innerInvoker)
        {
            this.actionService = actionService;
            this.actionContext = actionContext;
            this.innerInvoker = innerInvoker;
        }

        public void Invoke()
        {
            try
            {
                innerInvoker?.Invoke();
            }
            finally
            {
                actionService.Update(actionContext.ActionObject);
            }
            
        }
    }
}
