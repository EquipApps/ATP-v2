using DynamicData;
using EquipApps.Mvc;
using EquipApps.Mvc.Objects;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Services
{
    /// <summary>
    /// Singleton
    /// </summary>
    public interface IActionService
    {
        IObservableCache<ActionDescriptor, TestNumber> Observable { get; }

        Task UpdateAsync();

        Task CleanAsync();
    }
}
