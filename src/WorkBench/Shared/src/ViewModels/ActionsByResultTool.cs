using EquipApps.WorkBench.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionsByResultTool : ToolViewModel
    {
        public ActionsByResultTool( ActionsByResultViewer actionsByResultViewer)
            :base(nameof(ActionsByResultTool))
        {
            Viewer = actionsByResultViewer ?? throw new ArgumentNullException(nameof(actionsByResultViewer));

            ContentId = nameof(ActionsByResultTool);

            CanClose  = false;
            CanFloat  = true;
            CanHide   = true;
        }

        public ActionsByResultViewer Viewer { get; }
    }
}
