using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Mvc.Internal
{
    public class ViewActionInvokerProvider : IActionInvokerProvider
    {
        public int Order => throw new NotImplementedException();

        public void OnDisposeExecuted()
        {
            throw new NotImplementedException();
        }

        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
            throw new NotImplementedException();
        }

        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            throw new NotImplementedException();
        }
    }
}
