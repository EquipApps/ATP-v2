using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.Objects;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    public class DataContextModelBinder : IModelBinder
    {
        private int sourceIndex;

        public DataContextModelBinder(int sourceIndex)
        {
            this.sourceIndex = sourceIndex;
        }

        public BindingResult Bind(TestObject framworkElement, int offset = 0)
        {
            if (framworkElement == null)
            {
                throw new ArgumentNullException(nameof(framworkElement));
            }

            try
            {
                var index = sourceIndex - offset;
                var dataContext = framworkElement.GetDataContext(index);

                return BindingResult.Success(dataContext);

            }
            catch (Exception ex)
            {
                return BindingResult.Failed(ex);
            }
        }
    }
}
