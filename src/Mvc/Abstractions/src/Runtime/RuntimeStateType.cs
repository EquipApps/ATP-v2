namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Runtime State
    /// </summary>
    public enum RuntimeStateType : int
    {
        /// <summary>
        /// Состояние начала.
        /// </summary>
        START = 0,

        /// <summary>
        /// Состояние выполнения.
        /// </summary>
        INVOKE = 1,

        /// <summary>
        /// Состояние перемещения.
        /// </summary>
        MOVE = 2,

        /// <summary>
        /// Состояние завершения
        /// </summary>
        END = 3,
    }
}
