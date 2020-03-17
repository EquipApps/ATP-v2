using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class ParameterModel : IBindingModel
    {
        //---------------------------------------------------------------

        public ParameterModel(ParameterInfo info, IReadOnlyList<object> attributes)
        {
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            Info = info ?? throw new ArgumentNullException(nameof(info));

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
        /// Возвращает тип параметра
        /// </summary>
        public ParameterInfo Info { get; }

        /// <summary>
        /// Задает или возвращает <see cref="MethodModel"/>
        /// </summary>
        public MethodModel Method { get; set; }
        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/>
        /// </summary>
        public IBinder ModelBinder { get; set; }

        /// <summary>
        /// Задает или возвращает имя параметра
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IBindingModel Parent => Method;
    }
}