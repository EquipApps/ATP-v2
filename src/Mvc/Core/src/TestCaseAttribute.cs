using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Формерует Test Case 
    /// </summary>   
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CaseAttribute : Attribute, IDisplayFormatTitleMetadata, IDisplayIndexMetadata
    {
        public CaseAttribute(int number, string title)
        {
            Index = number;
            TitleFormat = title;
        }

        public CaseAttribute(string title)
        {
            TitleFormat = title;
        }


        public string TitleFormat { get; }


        public int? Index { get; }
    }
}
