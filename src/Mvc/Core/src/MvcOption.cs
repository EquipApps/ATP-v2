using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.Runtime;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc
{
    public class MvcOption
    {
        public MvcOption()
        {
            BindingProviders = new List<IBinderProvider>();
            RuntimeStates = new SortedList<RuntimeStateId, IRuntimeState>();
        }

        /// <summary>
        /// Стартовый индекс умолчанию.
        /// </summary>
        public int StartIndex { get; set; } = 1;





        /// <summary>
        /// Возвращает смписок <see cref="IBinderProvider"/>
        /// </summary>
        public List<IBinderProvider> BindingProviders { get; }

        public SortedList<RuntimeStateId, IRuntimeState> RuntimeStates { get; }
    }
}
