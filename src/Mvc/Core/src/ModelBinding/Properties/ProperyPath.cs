using EquipApps.Mvc.ModelBinding;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Properties
{
    /// <summary>
    /// Свойство.
    /// </summary>
    /// 
    /// <remarks>       
    /// Property
    /// Property.Property
    /// Property[0]
    /// Property[0].Property
    /// Property[0].Property[0]   
    /// </remarks>  
    public class ProperyPath
    {
        /// <summary>
        /// Полное имя свойства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задает или врозвращает коллекцию <see cref="ProperyPathItem"/>
        /// </summary>
        public IReadOnlyList<ProperyPathItem> Items { get; set; }

        /// <summary>
        /// Задает или врозвращает <see cref="BindingSourceOrder"/>
        /// </summary>

        public BindingSourceOrder Order { get; set; }       //TODO: Проверть нужна или нет ?
    }
}