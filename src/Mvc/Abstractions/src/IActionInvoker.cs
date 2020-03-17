namespace EquipApps.Mvc
{
    /// <summary>
    /// Определяет интерфейс для выполнения BDD действий
    /// </summary>
    public interface IActionInvoker
    {
        /// <summary>
        /// Выполняет BDD действие
        /// </summary>
        void Invoke();
    }
}
