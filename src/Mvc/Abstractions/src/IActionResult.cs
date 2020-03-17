namespace EquipApps.Mvc
{
    /// <summary>
    /// Определяет инфроструктуру результата.
    /// </summary>
    /// 
    /// <remarks>
    /// Pattent Command
    /// </remarks>
    public interface IActionResult
    {
        /// <summary>
        /// Выполняет результат операции метода.
        /// </summary>
        /// 
        /// <param name="context">
        /// Контекст действия
        /// </param>       
        void ExecuteResult(ActionContext context);
    }
}
