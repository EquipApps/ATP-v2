using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EquipApps.Mvc.ModelBinding.Metadata
{
    public class ModelPropertyCollection : ReadOnlyCollection<ModelMetadata>
    {
        public ModelPropertyCollection(IEnumerable<ModelMetadata> properties)
           : base(properties.ToList())
        {
        }

        public ModelMetadata this[string propertyName]
        {
            get
            {
                if (propertyName == null)
                {
                    throw new ArgumentNullException(nameof(propertyName));
                }

                for (var i = 0; i < Items.Count; i++)
                {
                    var property = Items[i];
                    if (string.Equals(property.PropertyName, propertyName, StringComparison.Ordinal))
                    {
                        return property;
                    }
                }

                return null;
            }
        }
    }
}