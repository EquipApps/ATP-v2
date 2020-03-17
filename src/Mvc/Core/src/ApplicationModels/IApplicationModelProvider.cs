namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Используется для кастомизации создания  <see cref="ApplicationModel"/>
    /// </summary>
    public interface IApplicationModelProvider
    {
        int Order { get; }

        void OnProvidersExecuting(ApplicationModelContext context);

        void OnProvidersExecuted(ApplicationModelContext context);
    }
}
