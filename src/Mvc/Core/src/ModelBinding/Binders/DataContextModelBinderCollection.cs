using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using EquipApps.Mvc.Objects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    public class DataContextModelBinderCollection : IBinder
    {
        private PropertyExtractor function;
        private int sourceIndex;

        public DataContextModelBinderCollection(PropertyExtractor function, int sourceIndex)
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

                var models = function(dataContext) as IEnumerable;

                var list = new List<BindingResult>();

                foreach (var model in models)
                {
                    list.Add(BindingResult.Success(model));
                }

                return BindingResult.Success(list.ToArray());
            }
            catch (Exception ex)
            {
                return BindingResult.Failed(ex);
            }
        }
    }
}
