using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель класса контроллера
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class ControllerModel : IBindingModel
    {
        public ControllerModel(
            TypeInfo controllerType,
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

            Actions = new List<ActionModel>();
            //ApiExplorer = new ApiExplorerModel();
            Attributes = new List<object>(attributes);
            ControllerProperties = new List<PropertyModel>();
            //Filters = new List<IFilterMetadata>();
            Properties = new Dictionary<object, object>();
            RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            //Selectors = new List<SelectorModel>();
        }

        /// <summary>
        /// Задает или возвращает <see cref="ApplicationModel"/>
        /// </summary>
        public ApplicationModel Application { get; set; }

        /// <summary>
        /// Задает или возвращает область/>
        /// </summary>
        [Obsolete("Use RouteValues")]
        public string Area { get; set; }

        /// <summary>
        /// Возвращает список аттрибутов
        /// </summary>
        public IReadOnlyList<object> Attributes { get; }

        /// <summary>
        /// Задает или возвращает <see cref="ApplicationModels.BindingInfo"/>.
        /// Может быть NULL.
        /// </summary>
        public BindingInfo BindingInfo { get; set; }

        /// <summary>
        /// Возвращает индекс. (Используется для сортировки)
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Возвращает <see cref="TypeInfo"/>
        /// </summary>
        public TypeInfo ControllerType { get; }

        /// <summary>
        /// Возвращает список <see cref="ActionModel"/>
        /// </summary>
        public IList<ActionModel> Actions { get; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/>.
        /// Может быть NULL.
        /// </summary>        
        public IModelBinder ModelBinder { get; set; }

        /// <summary>
        /// Возвращает имя (Используктся для навигации между проверками)
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Задает или возвращает имя
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/> для формирования номера.
        /// Может быть NULL.
        /// </summary>
        public IModelBinder NumberBinder { get; set; }

        /// <summary>
        /// Возвращает Null.. 
        /// Т.к контроллер является главнм самым верхним элементом с поддержкой привязки!
        /// </summary>
        public IBindingModel Parent => null;

        /// <summary>
        /// Возвращает список <see cref="PropertyModel"/>
        /// </summary>        
        public IList<PropertyModel> ControllerProperties { get; }

        /// <summary>
        /// Возвращает или задает заголовок
        /// </summary>    
        public string Title { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/> для формирования заголовка.
        /// Может быть NULL.
        /// </summary>
        public IModelBinder TitleBinder { get; set; }


        /// <summary>
        /// Gets a collection of route values that must be present in the 
        /// <see cref="RouteData.Values"/> for the corresponding action to be selected.
        /// </summary>
        /// <remarks>
        /// Entries in <see cref="RouteValues"/> can be overridden by entries in
        /// <see cref="ActionModel.RouteValues"/>.
        /// </remarks>
        public IDictionary<string, string> RouteValues { get; }

        /// <summary>
        /// Gets a set of properties associated with the controller.
        /// These properties will be copied to <see cref="Abstractions.ActionDescriptor.Properties"/>.
        /// </summary>
        /// <remarks>
        /// Entries will take precedence over entries with the same key
        /// in <see cref="ApplicationModel.Properties"/>.
        /// </remarks>
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
    }
}