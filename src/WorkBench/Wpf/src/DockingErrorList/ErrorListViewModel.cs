using EquipApps.Mvc.Reactive.LogsFeatures.Viewers;
using EquipApps.WorkBench.Docking;
using System;

namespace EquipApps.WorkBench.DockingErrorList
{
    public class ErrorListViewModel : ToolViewModel
    {
        public ErrorListViewModel(LogViewer logViewer)
            : base("Протокол проверки")
        {
            Viewer = logViewer ?? throw new ArgumentNullException(nameof(logViewer));

            ContentId = "LogViewer";

            CanClose = false;
            CanFloat = true;
            CanHide = true;
        }

        public LogViewer Viewer { get; }
    }
}
