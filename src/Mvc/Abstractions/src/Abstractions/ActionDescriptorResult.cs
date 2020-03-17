namespace EquipApps.Mvc.Abstractions
{
    // <summary>
    /// Результат
    /// </summary>
    public enum Result : byte
    {
        /// <summary>
        /// Не выполненно
        /// </summary>
        NotExecuted = 1,

        /// <summary>
        /// Прошло
        /// </summary>
        Passed = 2,

        /// <summary>
        /// Не реализованна
        /// </summary>
        NotImplemented = 3,

        /// <summary>
        ///  Прерванна
        /// </summary>
        Inconclusive = 4,

        /// <summary>
        /// Провалена
        /// </summary>
        Failed = 5
    }
}
