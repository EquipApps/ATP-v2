using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.Routing;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Формерует TestSuit
    /// </summary>   
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class StepAttribute : OrderValueAttribute, IDisplayFormatTitleMetadata, IDisplayFormatNumberMetadata
    {
        public StepAttribute(int number, string title)
            : base("action", number.ToString())
        {
            NumberFormat = number.ToString();
            TitleFormat = title;
        }

        public StepAttribute(string order, string title)
            : base("action", order)
        {
            NumberFormat = order;
            TitleFormat = title;
        }


        public string TitleFormat { get; }

        public string NumberFormat { get; }
    }
}
