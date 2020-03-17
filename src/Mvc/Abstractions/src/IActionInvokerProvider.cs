namespace EquipApps.Mvc
{
    public interface IActionInvokerProvider
    {
        int Order { get; }

        void OnProvideExecuted(ActionInvokerProviderContext context);
        void OnDisposeExecuted();
    }
}
