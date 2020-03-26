using NLib.AtpNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public abstract class ModelProvider<T> : IModelProvider<T>, IModelProvider where T : class
    {
        public abstract IReadOnlyList<T> Provide();

        IReadOnlyList<object> IModelProvider.Provide()
        {
            return Provide();
        }
    }
}
