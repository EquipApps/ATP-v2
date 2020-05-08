using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace EquipApps.WorkBench.Docking
{
    /// <summary>
    /// Singleton
    /// </summary>
    public class DockingViewModel : ReactiveObject
    {
        public DockingViewModel(IOptions<DockingOptions> options)
        {
            var option = options.Value;

            Files = new ObservableCollection<FileViewModel>(option.Files);
            Tools = new ObservableCollection<ToolViewModel>(option.Tools);
        }

        /// <summary>
        /// Файлы
        /// </summary>
        public ObservableCollection<FileViewModel> Files
        {
            get;
            private set;
        }

        /// <summary>
        /// Инструменты
        /// </summary>
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
