namespace EquipApps.Testing
{
    /// <summary>
    /// Маркер сущности.
    /// Используется для напсания расширений.
    /// </summary>
    public interface IEnableContext
    {
        TestContext TestContext { get; }
    }
}
