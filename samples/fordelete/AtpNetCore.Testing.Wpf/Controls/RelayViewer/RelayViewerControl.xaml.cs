using ReactiveUI;
using Splat;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Controls.RelayViewer
{
    /// <summary>
    /// Interaction logic for RelayViewerControl.xaml
    /// </summary>
    public partial class RelayViewerControl : ReactiveUserControl<RelayViewerViewModel>, IEnableLogger
    {
        public RelayViewerControl()
        {
            InitializeComponent();
            this.InitializeViewModel();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Items, x => x.RelayItemsControl.ItemsSource)
                    .DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Items, x => x.RelayDataGrid.ItemsSource)
                    .DisposeWith(disposable);

            });
        }


    }
}
