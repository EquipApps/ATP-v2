﻿namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActionInvokerProvider
    {
        int Order { get; }

        void OnProvidersExecuting(ActionInvokerProviderContext context);
        void OnProvidersExecuted(ActionInvokerProviderContext context);
        void OnDisposeExecuted();
    }
}
