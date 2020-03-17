namespace EquipApps.Mvc.Abstractions
{
    public abstract class ActionDescripterFilterBase : IActionDescriptorProvider
    {
        int IActionDescriptorProvider.Order => int.MaxValue;

        void IActionDescriptorProvider.OnProvidersExecuted(ActionDescriptorContext context)
        {
            //-- 
            OnFilterExecuted(context);
        }

        protected abstract void OnFilterExecuted(ActionDescriptorContext context);


        void IActionDescriptorProvider.OnProvidersExecuting(ActionDescriptorContext context)
        {
            //-- Ничего не делаем
        }
    }
}
