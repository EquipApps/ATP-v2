namespace NLib.AtpNetCore.Mvc.ModelBinding.Text
{
    /// <summary>
    /// Модель текста. с вставками цепочек свойств.
    /// </summary>
    public struct TextModel
    {
        /// <summary>
        /// Задает или возвращает формат текста.
        /// </summary>
        /// 
        /// <remarks>
        /// Формат представляет строку вида : "Текст {0} Текст {1} Текст ..."
        /// </remarks>       
        public string Format { get; set; }

        /// <summary>
        /// Задает или возвращает коллекцию свойств
        /// </summary>        
        public string[] Inserts { get; set; }
    }
}

