using ReactiveUI;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Controls.BatteryViewer
{
    /// <summary>
    /// Interaction logic for BatteryViewerControl.xaml
    /// </summary>
    public partial class BatteryViewerControl : ReactiveUserControl<BatteryViewerViewModel>
    {
        public BatteryViewerControl()
        {
            InitializeComponent();
            this.InitializeViewModel();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Items, x => x.BatteryListView.ItemsSource)
                    .DisposeWith(disposable);
            });
        }
    }
}
