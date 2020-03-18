using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for ActionsByResultView.xaml
    /// </summary>
    public partial class ActionsByResultView : ReactiveUserControl<ActionsByResultTool>
    {
        public ActionsByResultView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Viewer.Data, x => x.Data.ItemsSource).DisposeWith(disposable);
            });
        }
    }
}
