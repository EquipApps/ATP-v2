using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Формерует TestCase
    /// </summary>   
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class CaseAttribute : Attribute, IDisplayFormatTitleMetadata, IDisplayFormatNumberMetadata
    {
        public CaseAttribute(string number, string name)
        {
            NumberFormat = number;
            TitleFormat = name;
        }

        public CaseAttribute(string title)
        {
            TitleFormat = title;
        }


        public string TitleFormat { get; }

        public string NumberFormat { get; }
    }
}
