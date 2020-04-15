namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Состояния конвеера
    /// </summary>
    public enum RuntimeState : int
    {
        /// <summary>
        /// Начало
        /// </summary>
        Reset = 0,

        /// <summary>
        /// Состояние выполнения
        /// </summary>
        Invoke = 1,

        /// <summary>
        /// Состояние паузы
        /// </summary>
        Pause = 2,

        /// <summary>
        /// Состояние повтора. одного шага.
        /// </summary>
        RepeatOnce = 3,

        /// <summary>
        /// Состояние перехода
        /// </summary>
        Move = 4,

        /// <summary>
        /// Состояние повтора. цикл.
        /// </summary>
        Repeat = 5,

        /// <summary>
        /// Конец
        /// </summary>
        End = 6,
    }
}
