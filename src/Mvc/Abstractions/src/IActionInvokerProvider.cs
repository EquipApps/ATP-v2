namespace EquipApps.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActionInvokerProvider
    {
        int Order { get; }

        void OnProvideExecuted(ActionInvokerProviderContext context);
        void OnDisposeExecuted();
    }
}
