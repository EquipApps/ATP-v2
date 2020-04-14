namespace EquipApps.WorkBench.ViewModels
{
    /// <summary>
    /// Transient
    /// </summary>
    public partial class ActionsViewer : FileViewModel
    {
        partial void Initialize()
        {
            Title     = "Алгоритм проверки";
            ContentId = "TestViewer";

            CanClose    = false;
            CanFloat    = false;
            CanHide     = false;
        }
    }
}
