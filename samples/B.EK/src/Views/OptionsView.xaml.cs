using B.EK.ViewModels;
using MahApps.Metro.Controls;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace B.EK.Views
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    public partial class OptionsView : Flyout, IViewFor<OptionsViewModel>
    {
        public OptionsViewModel ViewModel
        {
            get => DataContext as OptionsViewModel;
            set => DataContext = value;
        }
        object IViewFor.ViewModel
        {
            get => DataContext;
            set => DataContext = value;
        }

        public OptionsView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.InitializeViewModel();

                //--
                this.Bind(this.ViewModel, x => x.IsOpen, x => x.IsOpen)
                    .DisposeWith(disposable);

                //--
                this.BindCommand(this.ViewModel, x => x.Accept, x => x.Accept)
                    .DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.Cancel, x => x.Cancel)
                    .DisposeWith(disposable);
               

                //--
                this.Bind(this.ViewModel, x => x.UserName,  x => x.UserName.Text)   .DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Number,    x => x.Number.Text)     .DisposeWith(disposable);

                //--
                this.OneWayBind(this.ViewModel, x => x.OperationModeCollection, x => x.WmType.ItemsSource)  .DisposeWith(disposable);
                this.Bind(this.ViewModel,       x => x.WorkingMode,   x => x.WmType.SelectedItem) .DisposeWith(disposable);
                
                //--
                this.OneWayBind(this.ViewModel, x => x.CheckModeIsEnabled,  x => x.CmType.IsEnabled)        .DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.CheckModeCollection, x => x.CmType.ItemsSource)      .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.ExecutingMode,   x => x.CmType.SelectedItem)     .DisposeWith(disposable);
                
                //--
                this.OneWayBind(this.ViewModel, x => x.PowerModeIsEnabled,  x => x.PmType.IsEnabled)        .DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.PowerModeCollection, x => x.PmType.ItemsSource)      .DisposeWith(disposable);
                this.Bind      (this.ViewModel, x => x.PowerMode,   x => x.PmType.SelectedItem)     .DisposeWith(disposable);

            });

        }
    }
}
