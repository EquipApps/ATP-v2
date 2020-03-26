using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель класса контроллера
    /// </summary>
    public class ControllerModel : IBindingModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ControllerModel(TypeInfo info, IReadOnlyList<object> attributes)
        {
            Info        = info ?? throw new ArgumentNullException(nameof(info));
            Attributes  = attributes ?? throw new ArgumentNullException(nameof(attributes));
            Methods     = new List<MethodModel>();
            Properties  = new List<PropertyModel>();

            Name = Info.Name;
        }

        /// <summary>
        /// Задает или возвращает <see cref="ApplicationModel"/>
        /// </summary>
        public ApplicationModel Application { get; set; }

        /// <summary>
        /// Задает или возвращает область/>
        /// </summary>
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
        public TypeInfo Info { get; }

        /// <summary>
        /// Возвращает список <see cref="MethodModel"/>
        /// </summary>
        public IList<MethodModel> Methods { get; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/>.
        /// Может быть NULL.
        /// </summary>        
        public IBinder ModelBinder { get; set; }

        /// <summary>
        /// Возвращает имя (Используктся для навигации между проверками)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задает или возвращает имя
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/> для формирования номера.
        /// Может быть NULL.
        /// </summary>
        public IBinder NumberBinder { get; set; }

        /// <summary>
        /// Возвращает Null.. 
        /// Т.к контроллер является главнм самым верхним элементом с поддержкой привязки!
        /// </summary>
        public IBindingModel Parent => null;

        /// <summary>
        /// Возвращает список <see cref="PropertyModel"/>
        /// </summary>        
        public IList<PropertyModel> Properties { get; }

        /// <summary>
        /// Возвращает или задает заголовок
        /// </summary>    
        public string Title { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/> для формирования заголовка.
        /// Может быть NULL.
        /// </summary>
        public IBinder TitleBinder { get; set; }
    }
}