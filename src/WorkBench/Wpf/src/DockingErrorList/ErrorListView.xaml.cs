using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.DockingErrorList
{
    /// <summary>
    /// Interaction logic for LogViewerControl.xaml
    /// </summary>
    public partial class LogViewerView : ReactiveUserControl<ErrorListViewModel>
    {
        public LogViewerView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                //TODO: Проверить. Нужна ли задержка или нет?

                this.ViewModel.WhenAnyValue(x => x.Viewer.CountFiltr.CountFail,
                                            x => x.Viewer.CountTotal.CountFail)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountFail.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Viewer.CountFiltr.CountWarn,
                                            x => x.Viewer.CountTotal.CountWarn)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountWarn.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.Viewer.CountFiltr.CountInfo,
                                            x => x.Viewer.CountTotal.CountInfo)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountInfo.Text)
                              .DisposeWith(disposable);

                this.Bind(ViewModel, x => x.Viewer.Filter.ShowFail, x => x.FilterFail.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.Filter.ShowWarn, x => x.FilterWarn.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.Filter.ShowInfo, x => x.FilterInfo.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.Filter.ShowDbug, x => x.FilterDbug.IsChecked).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Viewer.Filter.Groups,        x => x.FilterGroups.ItemsSource) .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.Viewer.Filter.GroupSelected, x => x.FilterGroups.SelectedItem).DisposeWith(disposable);


                this.OneWayBind(ViewModel, x => x.Viewer.Logs, x => x.LogRecordsListView.ItemsSource)
                    .DisposeWith(disposable);


                this.BindCommand(ViewModel, x => x.Viewer.Filter.Clear, x => x.FilterRemove).DisposeWith(disposable);

            });
        }

        private static string ToStringFomat((int filtr, int count) arg)
        {
            if (arg.filtr == arg.count)
                return arg.filtr.ToString();

            return $"{arg.filtr} из {arg.count}";
        }
    }
}
