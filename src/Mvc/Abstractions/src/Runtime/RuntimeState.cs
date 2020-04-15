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
        /// Подготовка
        /// </summary>
        Begin = 1,

        /// <summary>
        /// Состояние выполнения 
        /// </summary>
        Invoke = 2,

        /// <summary>
        /// Состояние паузы
        /// </summary>
        Pause = 3,

        /// <summary>
        /// Состояние повтора. одного шага.
        /// </summary>
        RepeatOnce = 4,

        /// <summary>
        /// Состояние перехода
        /// </summary>
        Move = 5,

        /// <summary>
        /// Состояние повтора. цикл.
        /// </summary>
        Repeat = 6,

        /// <summary>
        /// Конец
        /// </summary>
        End = 7,
    }
}
