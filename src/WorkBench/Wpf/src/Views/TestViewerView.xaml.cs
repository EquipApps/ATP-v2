using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestViewerView : ReactiveUserControl<TestViewerViewModel>
    {
        public TestViewerView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.Action.IsEnabledBreakPoint, x => x.Column_BreakPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);

                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.Action.IsEnabledBreakPoint, x => x.Column_CheckPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);





                //-- Привязка поллекции
                this.OneWayBind(this.ViewModel, x => x.Action.Data, x => x.dataGrid.ItemsSource)
                    .DisposeWith(disposable);

                //-- Привязка выбранного элемента
                this.Bind(this.ViewModel, x => x.Action.SelectedItem, x => x.dataGrid.SelectedItem)
                    .DisposeWith(disposable);

                //-- Привязка Команды фильтрации
                this.BindCommand(this.ViewModel, x => x.Action.Filter, x => x.dataGrid_cm_FilterButton)
                    .DisposeWith(disposable);
            });
        }

        private static Visibility ToVisibility(bool IsEnabled)
        {
            if (IsEnabled)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }
    }
}
