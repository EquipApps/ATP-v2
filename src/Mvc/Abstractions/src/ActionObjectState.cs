namespace EquipApps.Mvc
{
    /// <summary>
    /// Состояние
    /// </summary>
    public enum ActionObjectState : byte
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
