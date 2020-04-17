namespace EquipApps.Mvc
{
    public class GroupInfo
    {
        /// <summary>
        /// Задает флаг. (Показывать или нет сообщения с NULL контекстом)
        /// </summary>
        public bool ShowNullContext { get; set; }

        /// <summary>
        /// Задает флаг. (Показывать или нет сообщения с любым контекстом)
        /// </summary>
        public bool ShowManyContext { get; set; }

        /// <summary>
        /// ЗАголовок группы
        /// </summary>
        public string Title { get; set; }
    }
}
