namespace EquipApps.Testing.Features
{
    /// <summary>
    /// Инфроструктура провайдера расширений.
    /// (Вызывается каждый раз при создании тестовой проверки)
    /// </summary>  
    /// 
    /// <remarks>
    /// Используется для конфигурации <see cref="IFeatureCollection"/>
    /// </remarks>
    public interface IFeatureProvider
    {
        /// <summary>
        /// Порядковый номер провайдера.       
        /// </summary>
        /// 
        /// <remarks>
        /// Используется для сортировки <see cref="IFeatureProvider"/>.
        /// Позволяет настроить последовательность вызова.
        /// </remarks>
        int Order { get; }


        /// <summary>
        /// Выполнить. Прямой порядок
        /// </summary>
        /// 
        /// <param name="context">
        /// Контекст
        /// </param>
        void OnProvidersExecuting(FeatureProviderContext context);

        /// <summary>
        /// Выполнить. Обратныйы порядок
        /// </summary>
        /// 
        /// <param name="context">
        /// Контекст
        /// </param>
        void OnProvidersExecuted(FeatureProviderContext context);
    }
}
