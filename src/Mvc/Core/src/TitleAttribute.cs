using EquipApps.Mvc.ModelBinding;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TitleAttribute : Attribute, IDisplayFormatTitleMetadata
    {
        public TitleAttribute(string title)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));

            TitleFormat = title;
        }

        public string TitleFormat { get; }
    }
}
