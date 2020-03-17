namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Контекс. Используется в <see cref="IApplicationModelProvider"/> для создания <see cref="ApplicationModel"/>
    /// </summary>
    public class ApplicationModelContext
    {
        public ApplicationModel Result { get; } = new ApplicationModel();
    }
}
