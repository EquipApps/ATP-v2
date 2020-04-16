using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель контроллера
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public partial class ControllerModel : IBindingModel, IDisplayModel
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

        MemberInfo ICommonModel.MemberInfo => ControllerType;

        string ICommonModel.Name => ControllerName;

        #endregion
    }
}