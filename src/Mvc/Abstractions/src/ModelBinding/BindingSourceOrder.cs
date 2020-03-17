namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Способ выбора источника привязки
    /// </summary>
    public enum BindingSourceOrder
    {
        /// <summary>
        /// Последовательный c начала
        /// </summary>
        Ascending,

        /// <summary>
        /// Обратный с конца коллекции
        /// </summary>
        Descending,

        /// <summary>
        /// Извлекает источник под определенным индексом
        /// </summary>
        Index,
    }
}
