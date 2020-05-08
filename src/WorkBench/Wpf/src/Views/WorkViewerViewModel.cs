using EquipApps.Mvc.Reactive.WorkFeatures.Viewers;
using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using EquipApps.WorkBench.Docking;

namespace EquipApps.WorkBench.ViewModels
{
    public class WorkViewerViewModel : ToolViewModel
    {
        public WorkViewerViewModel(WorkViewer workViewer)
            :base("Панель управления")
        {
            

            ContentId = "WorkViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = false;

            Viewer = workViewer;
        }

        public WorkViewer Viewer { get; }
    }
}
