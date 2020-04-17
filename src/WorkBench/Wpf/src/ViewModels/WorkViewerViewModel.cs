using EquipApps.Mvc.Runtime;
using EquipApps.Testing;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewerViewModel : ToolViewModel
    {
        public WorkViewerViewModel(ITestFactory testFactory,
            
            IRuntimeService runtimeService)
            :base("Панель управления")
        {
            ctor(runtimeService, testFactory);

            ContentId = "WorkViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = false;
        }
    }
}
