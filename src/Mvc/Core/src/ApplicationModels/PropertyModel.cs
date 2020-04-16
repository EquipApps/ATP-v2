using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель свойства контроллера.
    /// </summary>
    [DebuggerDisplay("PropertyModel: Name={PropertyName}")]
    public class PropertyModel : ParameterModelBase, ICommonModel, IBindingModel
    {
        public PropertyModel(
            PropertyInfo propertyInfo,
            IReadOnlyList<object> attributes)
            : base(propertyInfo?.PropertyType, attributes)
        {
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        
        public PropertyModel(PropertyModel other)
            : base(other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Controller   = other.Controller;
            PropertyInfo = other.PropertyInfo;
        }

        /// <summary>
        /// Gets or sets the <see cref="ControllerModel"/> this <see cref="PropertyModel"/> is associated with.
        /// </summary>
        public ControllerModel Controller { get; set; }

        MemberInfo ICommonModel.MemberInfo => PropertyInfo;

        public new IDictionary<object, object> Properties => base.Properties;

        public new IReadOnlyList<object> Attributes => base.Attributes;

        public PropertyInfo PropertyInfo { get; }

        public string PropertyName
        {
            get => Name;
            set => Name = value;
        }

        IBindingModel IBindingModel.Parent => Controller;
    }
}