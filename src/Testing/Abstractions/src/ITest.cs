using System;
using System.Threading;
using System.Threading.Tasks;

namespace EquipApps.Testing
{
    /// <summary>
    /// Тестовая проверка
    /// </summary>
    /// 
    /// <remarks>
    /// Для создания экземпляра используйте <see cref="ITestFactory"/>.
    /// </remarks>
    public interface ITest : IDisposable
    {
        /// <summary>
        /// Запуск прцесса проверки.
        /// </summary>        
        Task ProcessAsync(CancellationToken cancellationToken);
    }
}
