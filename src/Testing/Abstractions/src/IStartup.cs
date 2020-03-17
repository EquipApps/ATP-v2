using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Инфроструктура тестового приложения
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Возвращает <see cref="IServiceProvider"/>
        /// </summary>     
        IServiceProvider GetServiceProvider();

        /// <summary>
        /// Конфигурация <see cref="ITestBuilder"/>
        /// </summary>
        void Configure(ITestBuilder builder);
    }
}
