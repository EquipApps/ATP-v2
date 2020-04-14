using EquipApps.Mvc.Viewers;
using EquipApps.WorkBench.Services;
using System;

namespace EquipApps.WorkBench.ViewModels
{
    public class TestExplorerTool : FileViewModel
    {
        public TestExplorerTool(ActionsViewer actionViewViewModel)
            :base()
        {
            Action = actionViewViewModel ?? throw new ArgumentNullException(nameof(actionViewViewModel));

            Title     = "Алгоритм проверки";
            ContentId = "TestViewer";

            CanClose  = false;
            CanFloat  = false;
            CanHide   = false;
        }  
        
        /// <summary>
        /// 
        /// </summary>
        public ActionsViewer Action { get; }
    }
}
