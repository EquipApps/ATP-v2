using System.Windows;

namespace EquipApps.WorkBench.Services
{
    /// <summary>
    /// Инфроструктура кастомизациии графичиского интерфейса
    /// </summary>
    public interface IMahAppsService : IShellContentService
    {
        FrameworkElement[] GetFlyouts();
    }
}
