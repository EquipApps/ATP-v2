namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Состояние
    /// </summary>
    public enum State : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Empy = 1,

        /// <summary>
        /// Выполняется
        /// </summary>
        Invoke,

        /// <summary>
        /// Пауза
        /// </summary>
        Pause,

        /// <summary>
        /// Точка остановки
        /// </summary>
        BreakPoint,
    }
}
