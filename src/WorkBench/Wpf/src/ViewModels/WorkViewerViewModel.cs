using EquipApps.Mvc.Runtime;
using EquipApps.Mvc.Services;
using EquipApps.Testing;
using EquipApps.WorkBench.Services;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewerViewModel : ToolViewModel
    {
        public WorkViewerViewModel(ITestFactory testFactory,
            IActionService actionDescripterService,
            ILogService logEntryService,
            IRuntimeService runtimeService)
            :base("Панель управления")
        {
            ctor(runtimeService, testFactory, actionDescripterService, logEntryService);

            ContentId = "WorkViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = false;
        }
    }
}
