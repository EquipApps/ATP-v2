namespace EquipApps.Mvc.Abstractions
{
    // <summary>
    /// Результат
    /// </summary>
    public enum ActionDescriptorResultType : byte
    {
        /// <summary>
        /// Не выполненно
        /// </summary>
        NotRun = 0,

        /// <summary>
        /// Прошло
        /// </summary>
        Passed = 1,

        /// <summary>
        /// Провалена
        /// </summary>
        Failed = 2
    }
}
