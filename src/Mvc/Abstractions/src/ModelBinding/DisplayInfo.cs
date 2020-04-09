using System.Collections.Generic;
using System.Linq;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Информвация о отображении данных
    /// </summary>   
    public class DisplayInfo
    {
        /// <summary>
        /// Номер
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string TitleFormat { get; set; }


        public static DisplayInfo GetDisplayInfo(IEnumerable<object> attributes)
        {
            //-- Создаем результат по умолчанию!
            var displayInfo = new DisplayInfo();
            var isdisplayInfoPresent = false;

            // Number
            foreach (var displayNumberAttribute in attributes.OfType<IDisplayFormatNumberMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayNumberAttribute?.NumberFormat != null)
                {
                    displayInfo.NumberFormat = displayNumberAttribute.NumberFormat;
                    break;
                }
            }

            // Title
            foreach (var displayTitleAttribute in attributes.OfType<IDisplayFormatTitleMetadata>())
            {
                isdisplayInfoPresent = true;
                if (displayTitleAttribute?.TitleFormat != null)
                {
                    displayInfo.TitleFormat = displayTitleAttribute.TitleFormat;
                    break;
                }
            }


            return isdisplayInfoPresent ? displayInfo : null;
        }
    }
}