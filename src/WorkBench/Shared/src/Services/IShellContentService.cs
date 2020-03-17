using System.Windows;

namespace EquipApps.WorkBench.Services
{
    public interface IShellContentService
    {
        FrameworkElement GetMainView();
        FrameworkElement GetMenuView();
        FrameworkElement GetStatusBar();
        FrameworkElement GetToolView();
    }
}
