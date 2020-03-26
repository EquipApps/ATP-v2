using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ModelBinding;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public class MvcOptions
    {
        public MvcOptions()
        {
            Conventions = new List<IApplicationModelConvention>();



            BindingProviders = new List<IBinderProvider>();
        }






        /// <summary>
        /// Gets a list of <see cref="IApplicationModelConvention"/> instances that will be applied to
        /// the <see cref="ApplicationModel"/> when discovering actions.
        /// </summary>
        public IList<IApplicationModelConvention> Conventions { get; }








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
