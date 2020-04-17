using EquipApps.Mvc.Runtime;
using EquipApps.Testing;

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
