using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель класса контроллера
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public partial class ControllerModel : IBindingModel
    {
        public ControllerModel(TypeInfo controllerType,
                               IReadOnlyList<object> attributes)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            ControllerType = controllerType;
            Attributes = new List<object>(attributes);

            Actions = new List<ActionModel>();        
            ControllerProperties = new List<PropertyModel>();
            Properties = new Dictionary<object, object>();
            RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            OrderValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        //public ControllerModel(ControllerModel other)
        //{
        //    if (other == null)
        //    {
        //        throw new ArgumentNullException(nameof(other));
        //    }

        //    ControllerName = other.ControllerName;
        //    ControllerType = other.ControllerType;

        //    // Still part of the same application
        //    Application = other.Application;

        //    // These are just metadata, safe to create new collections
        //    Attributes = new List<object>(other.Attributes);
            
        //    Actions = new List<ActionModel>(other.Actions.Select(a => new ActionModel(a) { Controller = this }));
        //    ControllerProperties = new List<PropertyModel>(other.ControllerProperties.Select(p => new PropertyModel(p) { Controller = this }));
        //    Properties = new Dictionary<object, object>(other.Properties);
        //    RouteValues = new Dictionary<string, string>(other.RouteValues, StringComparer.OrdinalIgnoreCase);
        //    OrderValues = new Dictionary<string, string>(other.OrderValues, StringComparer.OrdinalIgnoreCase);
        //}

        /// <summary>
        /// 
        /// </summary>
        public TypeInfo ControllerType { get; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<object> Attributes { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationModel Application { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<ActionModel> Actions { get; }

        /// <summary>
        /// 
        /// </summary>
        public IList<PropertyModel> ControllerProperties { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> OrderValues { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> RouteValues { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<object, object> Properties { get; }

        /// <summary>
        /// The DisplayName of this controller.
        /// </summary>
        public string DisplayName
        {
            get
            {
                var controllerType = TypeNameHelper.GetTypeDisplayName(ControllerType);
                var controllerAssembly = ControllerType.Assembly.GetName().Name;
                return $"{controllerType} ({controllerAssembly})";
            }
        }


        #region Binding

        /// <summary>
        /// Задает или возвращает <see cref="ModelBinding.BindingInfo"/>.
        /// Может быть NULL.
        /// </summary>
        public BindingInfo BindingInfo { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="ModelBinding.DisplayInfo"/>.
        /// Может быть NULL.
        /// </summary>
        public DisplayInfo DisplayInfo { get; set; }

        /// <summary>
        /// Возвращает Null.. 
        /// Т.к контроллер является главнм самым верхним элементом с поддержкой привязки!
        /// </summary>
        IBindingModel IBindingModel.Parent => null;

        #endregion
    }
}