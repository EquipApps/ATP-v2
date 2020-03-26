using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Формерует область (обединяет TestCase в TestSuit)
    /// </summary>   
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AreaAttribute : Attribute, IDisplayAreaMetadata
    {
        public AreaAttribute(string area)
        {
            Area = area;
        }

        /// <summary>
        /// Возвращает область
        /// </summary>
        public string Area { get; }
    }
}
