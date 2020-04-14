using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestExplorerToolView : ReactiveUserControl<ActionsViewer>
    {

        public TestExplorerToolView()
        {
            this.InitializeComponent();

            this.WhenActivated(disposable =>
            {
                //-- Привязка счетчика
                this.ViewModel.WhenAnyValue(x => x.Filter.ShowFailed,
                                            x => x.CountTotal.Failed)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalFailed.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Filter.ShowNotRun,
                                            x => x.CountTotal.NotRun)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalNotRun.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Filter.ShowPassed,
                                            x => x.CountTotal.Passed)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalPassed.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Filter.ShowFailed,
                                            x => x.Filter.ShowNotRun,
                                            x => x.Filter.ShowPassed,
                                            x => x.CountTotal.Failed,
                                            x => x.CountTotal.NotRun,
                                            x => x.CountTotal.Passed,
                                            x => x.CountTotal.Total)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.TotalTotal.Text)
                              .DisposeWith(disposable);

                //-- Привязка фильтра
                this.Bind(this.ViewModel, x => x.Filter.ShowFailed, x => x.FilterFailed.IsChecked).DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Filter.ShowNotRun, x => x.FilterNotRun.IsChecked).DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Filter.ShowPassed, x => x.FilterPassed.IsChecked).DisposeWith(disposable);

                //-- Привязка Команды
                this.BindCommand(ViewModel, x => x.Filter.Clear, x => x.FilterClear).DisposeWith(disposable);










                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.IsEnabledBreakPoint, x => x.Column_BreakPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);

                //-- Привязка Скрывает Column_BreakPoint 
                this.OneWayBind(this.ViewModel, x => x.IsEnabledBreakPoint, x => x.Column_CheckPoint.Visibility, ToVisibility)
                    .DisposeWith(disposable);





                //-- Привязка поллекции
                this.OneWayBind(this.ViewModel, x => x.Data, x => x.dataGrid.ItemsSource)
                    .DisposeWith(disposable);

                //-- Привязка выбранного элемента
                this.Bind(this.ViewModel, x => x.SelectedItem, x => x.dataGrid.SelectedItem)
                    .DisposeWith(disposable);

                //-- Привязка Команды фильтрации
                this.BindCommand(this.ViewModel, x => x.FilterLogs, x => x.dataGrid_cm_FilterButton)
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
