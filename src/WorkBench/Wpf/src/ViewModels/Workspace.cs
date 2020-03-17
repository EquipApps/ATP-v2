using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace EquipApps.WorkBench.ViewModels
{
    public class Workspace : ViewModelBase
    {
        static Workspace _this = new Workspace();

        public static Workspace This => _this;


        private Workspace()
        {
            Files = new ObservableCollection<FileViewModel>();
            Tools = new ObservableCollection<ToolViewModel>();
        }

        public ObservableCollection<FileViewModel> Files
        {
            get;
            private set;
        }

        public ObservableCollection<ToolViewModel> Tools
        {
            get;
            private set;
        }

        [Reactive]
        public FileViewModel ActiveDocument
        {
            get;
            set;
        }
    }
}
