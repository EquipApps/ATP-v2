using EquipApps.WorkBench.Services;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow()
        {


            var serviceLocator = Locator.Current;

            InitializeComponent();

            var mahAppsService = serviceLocator.GetService<IMahAppsService>();
            if (mahAppsService == null)
            {
                return;
            }

            var flyouts = mahAppsService.GetFlyouts();
            if (flyouts != null)
            {
                Flyouts = new FlyoutsControl();

                foreach (var flyout in flyouts)
                {
                    Flyouts.Items.Add(flyout);

                }
            }

            var mainView = mahAppsService.GetMainView();
            if (mainView != null)
            {
                contentControl.Content = mainView;
            }

            var menuView = mahAppsService.GetMenuView();
            if (menuView != null)
            {
                menuControl.Content = menuView;
            }

            var statusBarContent = mahAppsService.GetStatusBar();
            if (statusBarContent != null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }


            //-- Регистрация обработчика
            Interactions.InteractionCreateException.RegisterHandler(ErrorsHandler);



        }

        private async Task ErrorsHandler(InteractionContext<Exception, Unit> context)
        {
            string title = "Ошибка";
            string message = context.Input?.Message;
            var mySettings = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Theme,
            };

            //ThemeManager.ChangeThemeBaseColor(Application.Current, this.Name);


            await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);

            context.SetOutput(Unit.Default);
        }
    }
}
