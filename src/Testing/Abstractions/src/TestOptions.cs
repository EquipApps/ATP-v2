using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace EquipApps.Testing
{
    /// <summary>
    /// Опции тестовой проверки.
    /// Доступен через <see cref="IOptions{}>" />
    /// </summary>
    public class TestOptions
    {
        /// <summary>
        /// Имя продукта.. "Блок1"  "Прибор1"
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///  Код продукта.. "ШЮГИ.888222.111
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Версия продукта.. "ИЗМ1"
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// Версия для разработчика ПО
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Возвращает свойства. Используются для кастомизациии опций
        /// </summary>
        public Dictionary<object, object> Properties { get; set; } = new Dictionary<object, object>();
    }
}
