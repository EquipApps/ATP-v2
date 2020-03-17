using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for LogViewerControl.xaml
    /// </summary>
    public partial class LogViewerView : ReactiveUserControl<LogViewerViewModel>
    {
        public LogViewerView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.TotlalCount.CountFail, x => x.TotalFail.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.TotlalCount.CountWarn, x => x.TotalWarn.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.TotlalCount.CountInfo, x => x.TotalInfo.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.LevelCount.CountFail, x => x.CountFail.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.LevelCount.CountWarn, x => x.CountWarn.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.LevelCount.CountInfo, x => x.CountInfo.Text).DisposeWith(disposable);

                this.Bind(ViewModel, x => x.FilterLevel.ShowFail, x => x.FilterFail.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.FilterLevel.ShowWarn, x => x.FilterWarn.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.FilterLevel.ShowInfo, x => x.FilterInfo.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.FilterLevel.ShowDbug, x => x.FilterDbug.IsChecked).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.FilterGroup.Groups,        x => x.FilterGroups.ItemsSource) .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.FilterGroup.GroupSelected, x => x.FilterGroups.SelectedItem).DisposeWith(disposable);


                this.OneWayBind(ViewModel, x => x.Logs, x => x.LogRecordsListView.ItemsSource)
                    .DisposeWith(disposable);


                this.BindCommand(ViewModel, x => x.FilterRemove, x => x.FilterRemove).DisposeWith(disposable);

            });
        }
    }
}
