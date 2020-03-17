using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Формерует TestSuit
    /// </summary>   
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SuitAttribute : Attribute, IDisplayFormatTitleMetadata, IDisplayIndexMetadata
    {
        public SuitAttribute(int number, string title)
        {
            Index       = number;
            TitleFormat = title;
        }

        public SuitAttribute(string title)
        {
            TitleFormat = title;
        }


        public string TitleFormat { get; }
        

        public int? Index { get; }
    }
}
