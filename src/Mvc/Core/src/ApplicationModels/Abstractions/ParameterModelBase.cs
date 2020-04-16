using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// A model type for reading and manipulation properties and parameters.
    /// <para>
    /// Derived instances of this type represent properties and parameters for controllers.
    /// </para>
    /// </summary>
    public abstract class ParameterModelBase
    {
        protected ParameterModelBase(
            Type parameterType,
            IReadOnlyList<object> attributes)
        {
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Attributes = new List<object>(attributes ?? throw new ArgumentNullException(nameof(attributes)));
            Properties = new Dictionary<object, object>();
        }

        protected ParameterModelBase(ParameterModelBase other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            ParameterType = other.ParameterType;
            Attributes = new List<object>(other.Attributes);
            Properties = new Dictionary<object, object>(other.Properties);

            Name = other.Name;
            BindingInfo = other.BindingInfo == null ? null : new BindingInfo(other.BindingInfo);
        }

        public Type ParameterType { get; }

        public IReadOnlyList<object> Attributes { get; }

        public IDictionary<object, object> Properties { get; }

        public string Name { get; protected set; }

        /// <summary>
        /// Задает или возвращает <see cref="ModelBinding.BindingInfo"/>.
        /// Может быть NULL.
        /// </summary>
        public BindingInfo BindingInfo { get; set; }

        
    }
}