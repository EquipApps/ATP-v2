using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.Objects;
using System;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding
{
    public static class BindingEx
    {

        //TODO: Юнит тест. 
        public static object GetDataContext(this TreeObject framworkElement, int index)
        {
            if (framworkElement == null)
            {
                throw new ArgumentNullException(nameof(framworkElement));
            }

            if (index < 0)
            {
                throw new ArgumentException(nameof(index));
            }


            var i = 0;
            var element = framworkElement;

            while (element != null)
            {
                if (i == index)
                {
                    return element.DataContext;
                }

                i++;
                element = element.Parent;
            }


            throw new InvalidOperationException("!");
        }












        /// <summary>
        /// Возврвщает перечисление
        /// </summary>     
        public static IEnumerable<IBindingModel> GetParentElements(this IBindingModel bindable)
        {
            var parent = bindable.Parent;

            while (parent != null)
            {
                yield return parent;

                parent = parent.Parent;
            }

            yield break;
        }

        /// <summary>
        /// Возврвщает перечисление
        /// </summary>     
        public static IEnumerable<IBindingModel> GetElements(this IBindingModel bindable)
        {
            var parent = bindable;

            while (parent != null)
            {
                yield return parent;

                parent = parent.Parent;
            }

            yield break;
        }


        public static IBindingModel GetParent(this IBindingModel bindable, int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(nameof(index));
            }

            var i = 0;
            var parent = bindable.Parent;

            while (parent != null && i < index)
            {
                parent = parent.Parent;
                i++;
            }

            return parent;
        }
    }
}
