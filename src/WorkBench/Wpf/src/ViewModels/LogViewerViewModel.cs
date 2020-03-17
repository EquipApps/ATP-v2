using EquipApps.WorkBench.Models;
using EquipApps.WorkBench.Services;
using Microsoft.Extensions.Options;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class LogViewerViewModel : ToolViewModel
    {
        public LogViewerViewModel(ILogEntryService logEntriesService, IOptions<LogOptions>  logOptions)
            :base("Протокол проверки")
        {
            ctor(logEntriesService, logOptions?.Value);

            ContentId = "LogViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = true;
        }
    }
}
