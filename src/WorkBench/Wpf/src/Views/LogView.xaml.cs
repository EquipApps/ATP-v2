using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for LogViewerControl.xaml
    /// </summary>
    public partial class LogViewerView : ReactiveUserControl<LogViewModel>
    {
        public LogViewerView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.Viewer.TotalCount.CountFail, x => x.TotalFail.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Viewer.TotalCount.CountWarn, x => x.TotalWarn.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Viewer.TotalCount.CountInfo, x => x.TotalInfo.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Viewer.FiltrCount.CountFail, x => x.CountFail.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Viewer.FiltrCount.CountWarn, x => x.CountWarn.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Viewer.FiltrCount.CountInfo, x => x.CountInfo.Text).DisposeWith(disposable);

                this.Bind(ViewModel, x => x.Viewer.FilterLevel.ShowFail, x => x.FilterFail.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.FilterLevel.ShowWarn, x => x.FilterWarn.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.FilterLevel.ShowInfo, x => x.FilterInfo.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Viewer.FilterLevel.ShowDbug, x => x.FilterDbug.IsChecked).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Viewer.FilterGroup.Groups,        x => x.FilterGroups.ItemsSource) .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.Viewer.FilterGroup.GroupSelected, x => x.FilterGroups.SelectedItem).DisposeWith(disposable);


                this.OneWayBind(ViewModel, x => x.Viewer.Logs, x => x.LogRecordsListView.ItemsSource)
                    .DisposeWith(disposable);


                this.BindCommand(ViewModel, x => x.Viewer.FilterRemove, x => x.FilterRemove).DisposeWith(disposable);

            });
        }
    }
}
