using B.EK.Views;
using EquipApps.WorkBench.Services;
using System.Windows;

namespace B.EK.Services
{
    /// <summary>
    /// Кастомизация графичиского интерфейса
    /// </summary>
    public class MahAppsService : IMahAppsService
    {
        public MahAppsService()
        {

        }

        public FrameworkElement[] GetFlyouts()
        {
            return new FrameworkElement[]
            {
                new OptionsView()

            };
        }

        public FrameworkElement GetMainView()
        {
            return null;
        }

        public FrameworkElement GetMenuView()
        {
            return null;
        }

        public FrameworkElement GetStatusBar()
        {
            return null;
        }

        public FrameworkElement GetToolView()
        {
            return null;
        }

    }
}
