using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    public static class DisplayInfoBuilder
    {
        public static DisplayInfo GetDisplayInfo(IEnumerable<object> attributes)
        {
            //-- Создаем результат по умолчанию!
            var displayInfo = new DisplayInfo();
            var isdisplayInfoPresent = false;

            // Area
            foreach (var displayTitleAttribute in attributes.OfType<IDisplayAreaMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayTitleAttribute?.Area != null)
                {
                    displayInfo.Area = displayTitleAttribute.Area;
                    break;
                }
            }

            // Index
            foreach (var displayIndexAttribute in attributes.OfType<IDisplayIndexMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayIndexAttribute?.Index != null)
                {
                    displayInfo.Index = displayIndexAttribute.Index;
                    break;
                }
            }

            // Number
            foreach (var displayNumberAttribute in attributes.OfType<IDisplayFormatNumberMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayNumberAttribute?.NumberFormat != null)
                {
                    displayInfo.Number = displayNumberAttribute.NumberFormat;
                    break;
                }
            }

            // Title
            foreach (var displayTitleAttribute in attributes.OfType<IDisplayFormatTitleMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayTitleAttribute?.TitleFormat != null)
                {
                    displayInfo.Title = displayTitleAttribute.TitleFormat;
                    break;
                }
            }

            return isdisplayInfoPresent ? displayInfo : null;
        }
    }
}