using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

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
                //-- Привязка поллекции
                this.OneWayBind(this.ViewModel, x => x.Action.Items, x => x.dataGrid.ItemsSource)
                    .DisposeWith(disposable);

                //-- Привязка выбранного элемента
                this.Bind(this.ViewModel, x => x.Action.SelectedItem, x => x.dataGrid.SelectedItem)
                    .DisposeWith(disposable);

                //-- Привязка Команды фильтрации
                this.BindCommand(this.ViewModel, x => x.Action.Filter, x => x.dataGrid_cm_FilterButton)
                    .DisposeWith(disposable);
            });
        }
    }
}
