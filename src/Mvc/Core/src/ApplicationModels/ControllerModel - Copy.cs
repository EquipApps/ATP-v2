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
    
    public partial class ControllerModel : IBindingModel
    {
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

        /// Задает или возвращает <see cref="IModelBinder"/>.
        /// Может быть NULL.
        /// </summary>                public IModelBinder ModelBinder { get; set; }

      

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
        /// Возвращает или задает заголовок
        /// </summary>    
        public string Title { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IModelBinder"/> для формирования заголовка.
        /// Может быть NULL.
        /// </summary>
        public IModelBinder TitleBinder { get; set; }

    }
}