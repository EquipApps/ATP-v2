using EquipApps.WorkBench.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for WorkViewerView.xaml
    /// </summary>
    public partial class WorkViewerView : ReactiveUserControl<WorkViewerViewModel>
    {
        public WorkViewerView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {

                //-- Test Build Binding
                this.BindCommand(this.ViewModel, x => x.TestCreate, x => x.TestBuild)
                    .DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.TestClean, x => x.TestClean)
                    .DisposeWith(disposable);

                //-- Test Start Binding
                this.BindCommand(this.ViewModel, x => x.TestStart, x => x.TestStart)
                    .DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.TestStop, x => x.TestStop)
                    .DisposeWith(disposable);

                //-- Test mode Binding
                this.BindCommand(this.ViewModel, x => x.TestPause, x => x.TestPause)
                    .DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.IsPauseEnabled, x => x.TestPause.IsChecked)
                    .DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.TestRepeat, x => x.TestRepeat)
                    .DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.IsRepeatEnabled, x => x.TestRepeat.IsChecked)
                    .DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.TestRepeatOnce, x => x.TestRepeatOnce)
                    .DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.IsRepeatOnceEnabled, x => x.TestRepeatOnce.IsChecked)
                    .DisposeWith(disposable);

                //-- Test navigation Binding
                //this.BindCommand(this.ViewModel, x => x.TestPrevious, x => x.TestPrevious)
                //    .DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.TestReplay, x => x.TestReplay)
                    .DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.TestNext, x => x.TestNext)
                    .DisposeWith(disposable);



                //this.BindCommand(this.ViewModel, x => x.Setting, x => x.Setting)
                //   .DisposeWith(disposable);



            });
        }
    }
}
