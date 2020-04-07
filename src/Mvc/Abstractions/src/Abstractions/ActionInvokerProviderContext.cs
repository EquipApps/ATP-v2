using System;

namespace EquipApps.Mvc
{
    public class ActionInvokerProviderContext
    {
        public ActionInvokerProviderContext(ActionContext actionContext)
        {
            ActionContext = actionContext ?? throw new ArgumentNullException(nameof(actionContext));
        }

        public ActionContext ActionContext { get; }

        public IActionInvoker Result { get; set; }
    }
}
