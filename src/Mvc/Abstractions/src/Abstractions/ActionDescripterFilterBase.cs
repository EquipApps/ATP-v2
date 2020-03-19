namespace EquipApps.Mvc.Abstractions
{
    public abstract class ActionDescripterFilterBase : IActionProvider
    {
        int IActionProvider.Order => int.MaxValue;

        void IActionProvider.OnProvidersExecuted(ActionDescriptorContext context)
        {
            //-- 
            OnFilterExecuted(context);
        }

        protected abstract void OnFilterExecuted(ActionDescriptorContext context);


        void IActionProvider.OnProvidersExecuting(ActionDescriptorContext context)
        {
            //-- Ничего не делаем
        }
    }
}
