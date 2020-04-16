using EquipApps.Mvc.ModelBinding;

namespace EquipApps.Mvc.ApplicationModels
{
    public interface IDisplayModel : IBindingModel, ICommonModel
    {
        DisplayInfo DisplayInfo { get; }
    }
}