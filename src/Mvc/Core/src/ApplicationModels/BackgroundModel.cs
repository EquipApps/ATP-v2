using EquipApps.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels
{
    /// <summary>
    /// Модель привязки..
    /// </summary>
    public class BackgroundModel
    {
        public BackgroundModel(TypeInfo info, IReadOnlyList<object> attributes)
        {
            Info        = info ?? throw new ArgumentNullException(nameof(info));
            Attributes  = attributes ?? throw new ArgumentNullException(nameof(attributes));
            Name        = Info.Name;
        }

        /// <summary>
        /// Возвращает список аттрибутов
        /// </summary>
        public IReadOnlyList<object> Attributes { get; }

        /// <summary>
        /// Задает или возвращает <see cref="ControllerModel"/>
        /// </summary>
        public ControllerModel Controller { get; set; }

        /// <summary>
        /// Возвращает тип
        /// </summary>
        public TypeInfo Info { get; }

        /// <summary>
        /// Задает или возвращает имя
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
        /// Задает или возвращает заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или возвращает <see cref="IBinder"/> для формирования заголовка.
        /// Может быть NULL.
        /// </summary>
        public IBinder TitleBinder { get; set; }
    }
}
