namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Интерфейс провайдера <see cref="IBinder"/>.
    /// Используется для создания кастомной привязки.
    /// </summary>
    public interface IBinderProvider
    {
        /// <summary>
        /// Создает привязку
        /// </summary>
        /// 
        /// <param name="context">
        /// Контекст привязки
        /// </param>     
        IBinder GetBinder(BinderProviderContext context);
    }
}
