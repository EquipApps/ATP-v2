using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using EquipApps.Mvc.Objects;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    public class DataContextModelBinderPath : IBinder
    {
        private PropertyExtractor function;
        private int sourceIndex;

        public DataContextModelBinderPath(PropertyExtractor function, int sourceIndex)
        {
            this.function = function;
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

                var bindedModel = function(dataContext);

                return BindingResult.Success(bindedModel);

            }
            catch (Exception ex)
            {
                return BindingResult.Failed(ex);
            }
        }
    }
}
