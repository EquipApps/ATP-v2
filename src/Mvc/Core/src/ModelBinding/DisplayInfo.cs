namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Информвация о отображении данных
    /// </summary>   
    public class DisplayInfo
    {
        /// <summary>
        /// Индекс
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Область
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }
    }
}