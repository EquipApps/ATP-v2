namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    ///  Определяет интерфейс для кастомизации создания <see cref="ActionDescriptor"/>
    /// </summary>
    public interface IActionDescriptorProvider
    {
        int Order { get; }

        void OnProvidersExecuting(ActionDescriptorContext context);

        void OnProvidersExecuted(ActionDescriptorContext context);
    }
}
