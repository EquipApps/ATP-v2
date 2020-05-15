using ReactiveUI;
using System;
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

                this.ViewModel.WhenAnyValue(x => x.CountFiltr.Сountfail,
                                            x => x.CountTotal.Сountfail)                              
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountFail.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.CountFiltr.Сountwarn,
                                            x => x.CountTotal.Сountwarn)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountWarn.Text)
                              .DisposeWith(disposable);

                this.ViewModel.WhenAnyValue(x => x.CountFiltr.Сountinfo,
                                            x => x.CountTotal.Сountinfo)
                              .Select(ToStringFomat)
                              .BindTo(this, x => x.CountInfo.Text)
                              .DisposeWith(disposable);

                this.Bind(ViewModel, x => x.Filter.ShowFail, x => x.FilterFail.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Filter.ShowWarn, x => x.FilterWarn.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Filter.ShowInfo, x => x.FilterInfo.IsChecked).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Filter.ShowDbug, x => x.FilterDbug.IsChecked).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Filter.Groups,        x => x.FilterGroups.ItemsSource) .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.Filter.GroupSelected, x => x.FilterGroups.SelectedItem).DisposeWith(disposable);


                this.OneWayBind(ViewModel, x => x.Logs, x => x.LogRecordsListView.ItemsSource)
                    .DisposeWith(disposable);


                this.BindCommand(ViewModel, x => x.Filter.Clear, x => x.FilterRemove).DisposeWith(disposable);

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
