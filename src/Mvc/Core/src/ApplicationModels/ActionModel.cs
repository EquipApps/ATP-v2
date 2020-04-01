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
    /// An application model for controller actions.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class ActionModel : IBindingModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ActionModel"/>.
        /// </summary>
        /// <param name="actionMethod">The action <see cref="MethodInfo"/>.</param>
        /// <param name="attributes">The attributes associated with the action.</param>
        public ActionModel(
            MethodInfo actionMethod,
            IReadOnlyList<object> attributes)
        {
            if (actionMethod == null)
            {
                throw new ArgumentNullException(nameof(actionMethod));
            }

            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            ActionMethod = actionMethod;

            //ApiExplorer = new ApiExplorerModel();
            Attributes = new List<object>(attributes);
            //Filters = new List<IFilterMetadata>();
            Parameters = new List<ParameterModel>();
            RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Properties = new Dictionary<object, object>();
            //Selectors = new List<SelectorModel>();
        }

        /// <summary>
        /// Copy constructor for <see cref="ActionModel"/>.
        /// </summary>
        /// <param name="other">The <see cref="ActionModel"/> to copy.</param>
        public ActionModel(ActionModel other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            ActionMethod = other.ActionMethod;
            ActionName = other.ActionName;
            //RouteParameterTransformer = other.RouteParameterTransformer;

            // Not making a deep copy of the controller, this action still belongs to the same controller.
            Controller = other.Controller;

            // These are just metadata, safe to create new collections
            Attributes = new List<object>(other.Attributes);
            //Filters = new List<IFilterMetadata>(other.Filters);
            Properties = new Dictionary<object, object>(other.Properties);
            RouteValues = new Dictionary<string, string>(other.RouteValues, StringComparer.OrdinalIgnoreCase);

            // Make a deep copy of other 'model' types.
            //ApiExplorer = new ApiExplorerModel(other.ApiExplorer);
            Parameters = new List<ParameterModel>(other.Parameters.Select(p => new ParameterModel(p) { Action = this }));
            //Selectors = new List<SelectorModel>(other.Selectors.Select(s => new SelectorModel(s)));
        }

        

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
        /// Задает или возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel Controller { get; set; }

        /// <summary>
        /// Возвращает <see cref="MethodInfo"/>
        /// </summary>
        public MethodInfo ActionMethod { get; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/>
        /// </summary>
        public IModelBinder ModelBinder { get; set; }

        /// <summary>
        /// Возвращает имя (Используктся для навигации между проверками)
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/> для формирования номера действия.
        /// </summary>
        public IModelBinder NumberBinder { get; set; }

        /// <summary>
        /// Возвращает список параметров действия
        /// </summary>
        public IList<ParameterModel> Parameters { get; }

        /// <summary>
        /// 
        /// </summary>
        public IBindingModel Parent => Controller;

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/> для формирования заголовка действия.
        /// </summary>
        public IModelBinder TitleBinder { get; set; }


        /// <summary>
        /// Gets a collection of route values that must be present in the 
        /// <see cref="RouteData.Values"/> for the corresponding action to be selected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of <see cref="ActionName"/> is considered an implicit route value corresponding
        /// to the key <c>action</c> and the value of <see cref="ControllerModel.ControllerName"/> is
        /// considered an implicit route value corresponding to the key <c>controller</c>. These entries
        /// will be implicitly added to <see cref="ActionDescriptor.RouteValues"/> when the action
        /// descriptor is created, but will not be visible in <see cref="RouteValues"/>.
        /// </para>
        /// <para>
        /// Entries in <see cref="RouteValues"/> can override entries in
        /// <see cref="ControllerModel.RouteValues"/>.
        /// </para>
        /// </remarks>
        public IDictionary<string, string> RouteValues { get; }

        /// <summary>
        /// Gets a set of properties associated with the action.
        /// These properties will be copied to <see cref="Abstractions.ActionDescriptor.Properties"/>.
        /// </summary>
        /// <remarks>
        /// Entries will take precedence over entries with the same key in
        /// <see cref="ApplicationModel.Properties"/> and <see cref="ControllerModel.Properties"/>.
        /// </remarks>
        public IDictionary<object, object> Properties { get; }


        /// <summary>
        /// Gets the action display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (Controller == null)
                {
                    return ActionMethod.Name;
                }

                var controllerType = TypeNameHelper.GetTypeDisplayName(Controller.ControllerType);
                var controllerAssembly = Controller?.ControllerType.Assembly.GetName().Name;
                return $"{controllerType}.{ActionMethod.Name} ({controllerAssembly})";
            }
        }
    }
}