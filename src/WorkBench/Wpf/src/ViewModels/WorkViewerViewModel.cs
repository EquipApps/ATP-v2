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
            ILogEntryService logEntryService,
            IOptions<MvcOption> options)
            :base("Панель управления")
        {
            ctor(testFactory, actionDescripterService, logEntryService, options);

            ContentId = "WorkViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = false;
        }
    }
}
