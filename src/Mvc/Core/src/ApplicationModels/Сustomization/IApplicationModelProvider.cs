namespace EquipApps.Mvc.ApplicationModels.Сustomization
{
    /// <summary>
    /// Используется для кастомизации создания  <see cref="ApplicationModel"/>
    /// </summary>
    public interface IApplicationModelProvider
    {
        int Order { get; }

        void OnProvidersExecuting(ApplicationModelProviderContext context);

        void OnProvidersExecuted(ApplicationModelProviderContext context);
    }
}
