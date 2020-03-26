using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class ActionModel : IBindingModel
    {
        public ActionModel(MethodInfo info, IReadOnlyList<object> attributes)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));

            Parameters = new List<ParameterModel>();
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
        public MethodInfo Info { get; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/>
        /// </summary>
        public IBinder ModelBinder { get; set; }

        /// <summary>
        /// Возвращает имя (Используктся для навигации между проверками)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/> для формирования номера действия.
        /// </summary>
        public IBinder NumberBinder { get; set; }

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
        /// Задает или возвращает <see cref="IBinder"/> для формирования заголовка действия.
        /// </summary>
        public IBinder TitleBinder { get; set; }
    }
}