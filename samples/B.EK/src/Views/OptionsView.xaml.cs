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
                this.Bind(this.ViewModel, x => x.UserName,  x => x.UserName.Text)  
                    .DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.Number,    x => x.Number.Text)    
                    .DisposeWith(disposable);

                //--
                this.Bind(this.ViewModel,       x => x.WorkingMode,   x => x.WorkingMode.SelectedValue)
                    .DisposeWith(disposable);
                
                //--
                this.OneWayBind(this.ViewModel, x => x.ExecutingModeIsEnabled,  x => x.ExecutingMode.IsEnabled)     
                    .DisposeWith(disposable);               
                this.Bind      (this.ViewModel, x => x.ExecutingMode,           x => x.ExecutingMode.SelectedValue)  
                    .DisposeWith(disposable);
                
                //--
                this.OneWayBind(this.ViewModel, x => x.PowerModeIsEnabled,  x => x.PowerMode.IsEnabled)       
                    .DisposeWith(disposable);              
                this.Bind      (this.ViewModel, x => x.PowerMode,           x => x.PowerMode.SelectedValue)     
                    .DisposeWith(disposable);

            });

        }
    }
}
