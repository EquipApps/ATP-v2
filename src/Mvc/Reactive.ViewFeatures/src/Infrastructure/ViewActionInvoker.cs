using EquipApps.Mvc.Reactive.ViewFeatures.Services;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Infrastructure
{
    /// <summary>
    /// Обертка.
    /// Обновлять состояние <see cref="ActionObject"/> после выполения.
    /// </summary>
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
                //-- Обновляет состояние
                actionService.Update(actionContext.ActionObject);
            }

        }
    }
}
