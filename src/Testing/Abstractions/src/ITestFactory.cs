using System.Threading.Tasks;

namespace EquipApps.Testing
{
    /// <summary>
    /// Фабрика тестовых проверок
    /// </summary>
    public interface ITestFactory
    {
        /// <summary>
        /// Создает <see cref="ITest"/> асинхронно.
        /// </summary>        
        Task<ITest> CreateTestAsync();

        /// <summary>
        /// Создает <see cref="ITest"/>.
        /// </summary>
        ITest CreateTest();
    }
}
