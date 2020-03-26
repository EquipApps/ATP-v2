using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Формерует TestSuit
    /// </summary>   
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class StepAttribute : Attribute, IDisplayFormatTitleMetadata, IDisplayFormatNumberMetadata
    {
        public StepAttribute(int number, string title)
        {
            NumberFormat = number.ToString();
            TitleFormat = title;
        }

        public StepAttribute(string number, string title)
        {
            NumberFormat = number;
            TitleFormat = title;
        }


        public string TitleFormat { get; }

        public string NumberFormat { get; }
    }
}
