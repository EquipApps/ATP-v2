namespace EquipApps.Mvc.Abstractions
{
    public abstract class ActionDescripterFilterBase : IActionDescriptorProvider
    {
        int IActionDescriptorProvider.Order => int.MaxValue;

        void IActionDescriptorProvider.OnProvidersExecuted(ActionDescriptorProviderContext context)
        {
            //-- 
            OnFilterExecuted(context);
        }

        protected abstract void OnFilterExecuted(ActionDescriptorProviderContext context);


        void IActionDescriptorProvider.OnProvidersExecuting(ActionDescriptorProviderContext context)
        {
            //-- Ничего не делаем
        }
    }
}
