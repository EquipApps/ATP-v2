using EquipApps.WorkBench.Services;
using System;

namespace EquipApps.WorkBench.ViewModels
{
    public class TestViewerViewModel : FileViewModel
    {
        public TestViewerViewModel(ActionViewModel actionViewViewModel)
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
        public ActionViewModel Action { get; }
    }
}
