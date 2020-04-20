namespace EquipApps.Mvc.Reactive.WorkFeatures
{
    /// <summary>
    /// Опции.
    /// </summary>
    public class RuntimeOptions
    {
        /// <summary>
        /// Число повторов.
        /// "-1" Соотвецтвует бесконечному количеству.
        /// </summary>
        public int RepetCount { get; set; } = -1;

        /// <summary>
        /// Задержка между повторами в милисекундах.
        /// </summary>
        public int RepetTimeout { get; set; } = -1;
    }
}
