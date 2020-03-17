using EquipApps.WorkBench.Services;
using System;

namespace EquipApps.WorkBench.ViewModels
{
    public class TestViewerViewModel : FileViewModel
    {
        public TestViewerViewModel(ActionViewViewModel actionViewViewModel)
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
        public ActionViewViewModel Action { get; }
    }
}
