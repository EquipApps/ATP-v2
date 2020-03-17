using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    public class PropertyModel : IBindingModel
    {
        public PropertyModel(PropertyInfo info, IReadOnlyList<object> attributes)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Возвращает список аттрибутов свойста
        /// </summary>
        public IReadOnlyList<object> Attributes { get; }

        /// <summary>
        /// Задает или возвращает <see cref="ModelBinding.BindingInfo"/>.
        /// </summary>
        public BindingInfo BindingInfo { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel Controller { get; set; }

        /// <summary>
        /// Возвращает тип свойства
        /// </summary>
        public PropertyInfo Info { get; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/>
        /// </summary>
        public IBinder ModelBinder { get; set; }

        /// <summary>
        /// Задает или возвращает имя свойства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IBindingModel Parent => Controller;
    }
}