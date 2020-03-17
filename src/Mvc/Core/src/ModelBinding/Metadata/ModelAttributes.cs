using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Metadata
{
    public class ModelAttributes
    {
        public ModelAttributes(IEnumerable<object> typeAttributes, IEnumerable<object> propertyAttributes, IEnumerable<object> parameterAttributes)
        {
            if (propertyAttributes != null)
            {
                // Represents a property
                if (typeAttributes == null)
                {
                    throw new ArgumentNullException(nameof(typeAttributes));
                }

                PropertyAttributes = propertyAttributes.ToArray();
                TypeAttributes = typeAttributes.ToArray();
                Attributes = PropertyAttributes.Concat(TypeAttributes).ToArray();
            }
            else if (parameterAttributes != null)
            {
                // Represents a parameter
                Attributes = ParameterAttributes = parameterAttributes.ToArray();
            }
            else if (typeAttributes != null)
            {
                // Represents a type
                if (typeAttributes == null)
                {
                    throw new ArgumentNullException(nameof(typeAttributes));
                }

                Attributes = TypeAttributes = typeAttributes.ToArray();
            }
        }


        public IReadOnlyList<object> Attributes { get; }

        public IReadOnlyList<object> PropertyAttributes { get; }

        public IReadOnlyList<object> ParameterAttributes { get; }

        public IReadOnlyList<object> TypeAttributes { get; }



        public static ModelAttributes GetAttributesForType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var attributes = type.GetTypeInfo().GetCustomAttributes();

            return new ModelAttributes(attributes, null, null);
        }

        public static ModelAttributes GetAttributesForProperty(Type type, PropertyInfo property)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var propertyAttributes = property.GetCustomAttributes();
            var typeAttributes = property.PropertyType.GetTypeInfo().GetCustomAttributes();


            return new ModelAttributes(typeAttributes, propertyAttributes, null);
        }

        public static ModelAttributes GetAttributesForParameter(ParameterInfo parameter)
        {
            return new ModelAttributes(null, null, parameter.GetCustomAttributes());
        }
    }
}
