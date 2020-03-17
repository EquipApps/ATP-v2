using System;

namespace EquipApps.Mvc
{
    public interface IActionInvokerFactory : IDisposable
    {
        IActionInvoker CreateInvoker(ActionContext actionContext);
    }
}
