using EquipApps.Mvc.ModelBinding;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// An application model for controller actions.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class ActionModel : IBindingModel, IDisplayModel
    {
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

            Attributes = new List<object>(attributes);            
            Parameters = new List<ParameterModel>();
            OrderValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Properties = new Dictionary<object, object>();
            
        }


        //public ActionModel(ActionModel other)
        //{
        //    if (other == null)
        //    {
        //        throw new ArgumentNullException(nameof(other));
        //    }

        //    ActionMethod = other.ActionMethod;
        //    ActionName = other.ActionName;
        //    //RouteParameterTransformer = other.RouteParameterTransformer;

        //    // Not making a deep copy of the controller, this action still belongs to the same controller.
        //    Controller = other.Controller;

        //    // These are just metadata, safe to create new collections
        //    Attributes = new List<object>(other.Attributes);
        //    //Filters = new List<IFilterMetadata>(other.Filters);
        //    Properties = new Dictionary<object, object>(other.Properties);
        //    RouteValues = new Dictionary<string, string>(other.RouteValues, StringComparer.OrdinalIgnoreCase);

        //    // Make a deep copy of other 'model' types.
        //    //ApiExplorer = new ApiExplorerModel(other.ApiExplorer);
        //    Parameters = new List<ParameterModel>(other.Parameters.Select(p => new ParameterModel(p) { Action = this }));
        //    //Selectors = new List<SelectorModel>(other.Selectors.Select(s => new SelectorModel(s)));
        //}

        /// <summary>
        /// Возвращает <see cref="MethodInfo"/>
        /// </summary>
        public MethodInfo ActionMethod { get; }

        /// <summary>
        /// Возвращает список аттрибутов
        /// </summary>
        public IReadOnlyList<object> Attributes { get; }

        /// <summary>
        /// Возвращает имя
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel Controller { get; set; }

        /// <summary>
        /// Возвращает список параметров действия
        /// </summary>
        public IList<ParameterModel> Parameters { get; }

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
        IBindingModel IBindingModel.Parent => Controller;

        /// <summary>
        /// 
        /// </summary>
        MemberInfo ICommonModel.MemberInfo => ActionMethod;

        /// <summary>
        /// 
        /// </summary>
        string ICommonModel.Name => ActionName;

        #endregion
    }
}