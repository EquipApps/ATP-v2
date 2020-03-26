using EquipApps.Mvc.ModelBinding;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public class MvcOption
    {
        public MvcOption()
        {
            BindingProviders = new List<IBinderProvider>();
        }

        /// <summary>
        /// Стартовый индекс умолчанию.
        /// </summary>
        public int StartIndex { get; set; } = 1;

        /// <summary>
        /// Возвращает смписок <see cref="IBinderProvider"/>
        /// </summary>
        public List<IBinderProvider> BindingProviders { get; }


    }
}
