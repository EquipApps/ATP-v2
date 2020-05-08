using ReactiveUI.Fody.Helpers;

namespace EquipApps.WorkBench.Docking
{
    public abstract class ToolViewModel : DockViewModel
    {
        public ToolViewModel(string name)
        {
            Name = name;
            Title = name;
        }

        public string Name { get; private set; }

        [Reactive]
        public bool IsVisible { get; set; } = true;
    }
}
