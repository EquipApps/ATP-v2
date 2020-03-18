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
        Empy = 0,

        /// <summary>
        /// Выполняется
        /// </summary>
        Invoke = 1,

        /// <summary>
        /// Пауза
        /// </summary>
        Pause = 2,

        /// <summary>
        /// Точка остановки
        /// </summary>
        BreakPoint = 3,
    }
}
