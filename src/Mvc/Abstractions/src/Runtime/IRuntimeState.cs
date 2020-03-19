namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Паттерн State
    /// </summary>
    public interface IRuntimeState
    {
        /// <summary>
        /// Обработка контекста
        /// </summary>
        void Handle(RuntimeContext context);
    }
}
