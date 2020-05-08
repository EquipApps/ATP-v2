using EquipApps.Mvc.Reactive.ViewFeatures.Viewers;
using EquipApps.WorkBench.Docking;

namespace EquipApps.WorkBench.DockingTestExplorer
{
    /// <summary>
    /// Transient
    /// </summary>
    public class TestExplorerViewModel : FileViewModel
    {
        public TestExplorerViewModel(ActionsViewer actions)
        {
            Actions = actions;

            Title = "Алгоритм проверки";
            ContentId = "TestViewer";

            CanClose = false;
            CanFloat = false;
            CanHide = false;

        }

        public ActionsViewer Actions { get; }
    }
}
