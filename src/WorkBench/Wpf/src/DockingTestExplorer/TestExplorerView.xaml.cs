using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;

namespace EquipApps.WorkBench.DockingTestExplorer
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestExplorerView : ReactiveUserControl<TestExplorerViewModel>
    {

        public TestExplorerView()
        {
            this.InitializeComponent();

            this.WhenActivated(disposable =>
            {
                //-- Привязка счетчика
                this.ViewModel.WhenAnyValue(x => x.Actions.Filter.ShowFailed,
                                            x => x.Actions.CountTotal.Failed)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalFailed.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Actions.Filter.ShowNotRun,
                                            x => x.Actions.CountTotal.NotRun)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalNotRun.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Actions.Filter.ShowPassed,
                                            x => x.Actions.CountTotal.Passed)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalPassed.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Actions.Filter.ShowFailed,
                                            x => x.Actions.Filter.ShowNotRun,
                                            x => x.Actions.Filter.ShowPassed,
                                            x => x.Actions.CountTotal.Failed,
                                            x => x.Actions.CountTotal.NotRun,
                                            x => x.Actions.CountTotal.Passed,
                                            x => x.Actions.CountTotal.Total)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalTotal.Text)
                              .DisposeWith(disposable);

                //-- Привязка фильтра
                this.Bind(this.ViewModel, x => x.Actions.Filter.ShowFailed, x => x.FilterFailed.IsChecked).DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Actions.Filter.ShowNotRun, x => x.FilterNotRun.IsChecked).DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Actions.Filter.ShowPassed, x => x.FilterPassed.IsChecked).DisposeWith(disposable);

                //-- Привязка Команды
                this.BindCommand(ViewModel, x => x.Actions.Filter.Clear, x => x.FilterClear).DisposeWith(disposable);










                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.Actions.IsEnabledBreakPoint, x => x.Column_BreakPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);

                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.Actions.IsEnabledBreakPoint, x => x.Column_CheckPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);





                //-- Привязка поллекции
                this.OneWayBind(this.ViewModel, x => x.Actions.Data, x => x.dataGrid.ItemsSource)
                    .DisposeWith(disposable);

                //-- Привязка выбранного элемента
                this.Bind(this.ViewModel, x => x.Actions.SelectedItem, x => x.dataGrid.SelectedItem)
                    .DisposeWith(disposable);

                //-- Привязка Команды фильтрации
                this.BindCommand(this.ViewModel, x => x.Actions.FilterLogs, x => x.dataGrid_cm_FilterButton)
                    .DisposeWith(disposable);
            });
        }

        private static string ToStringFomat((bool showFailed, bool showNotRun, bool showPassed, int failed, int notRun, int passed, int total) arg)
        {      
            var data    = 0;

            if (arg.showFailed)
            {
                data += arg.failed;
            }
            if (arg.showNotRun)
            {
                data += arg.notRun;
            }
            if (arg.showPassed)
            {
                data += arg.passed;
            }

            return data == arg.total ? data.ToString() : $"{data}/{arg.total}";
        }

        private static object ToStringFomat((bool show, int count) arg)
        {
            if (arg.show)
                return arg.count.ToString();
            else
                return "0/" + arg.count.ToString();
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
