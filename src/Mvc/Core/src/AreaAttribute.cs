using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Формерует заголовок для TestSuit, TestCase, TestStep 
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
