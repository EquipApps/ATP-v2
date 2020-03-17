using ReactiveUI.Fody.Helpers;

namespace EquipApps.WorkBench.ViewModels
{
    public abstract class ToolViewModel : PaneViewModel
    {
        public ToolViewModel(string name)
        {
            Name  = name;
            Title = name;
        }

        public string Name { get; private set; }

        [Reactive]
        public bool IsVisible { get; set; } = true;
    }
}
