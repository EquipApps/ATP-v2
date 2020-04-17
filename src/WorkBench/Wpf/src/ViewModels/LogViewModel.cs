using EquipApps.Mvc.Reactive.LogsFeatures.Viewers;
using System;

namespace EquipApps.WorkBench.ViewModels
{
    public class LogViewModel : ToolViewModel
    {
        public LogViewModel(LogViewer logViewer)
            :base("Протокол проверки")
        {
            Viewer = logViewer ?? throw new ArgumentNullException(nameof(logViewer));

            ContentId = "LogViewer";

            CanClose = false;
            CanFloat = true;
            CanHide  = true;
        }

        public LogViewer Viewer { get; }
    }
}
