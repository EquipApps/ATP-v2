using EquipApps.Mvc.ModelBinding;
using NLib.AtpNetCore.Mvc.ModelBinding.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Properties
{
    public static class PropertyParser
    {

        /// <summary>
        /// Разбирает путь по частям
        /// </summary>
        /// 
        /// <param name="properyPath">     
        /// Формат пути:      
        /// Property
        /// Property.Property
        /// Property[0]
        /// Property[0].Property
        /// Property[0].Property[0]   
        /// </param>
        /// 
        /// <returns>
        /// Property Property [0]
        /// </returns>
        public static IReadOnlyList<ProperyPathItem> ParseProperyPath(string properyPath)
        {
            if (properyPath == null)
            {
                throw new ArgumentException(nameof(properyPath));
            }

            var math = RegularExpressionHelper.ProperyRegex.Match(properyPath);
            if (!math.Success)
                throw new InvalidOperationException(string.Format("Путь {0} имеет не верный формат", properyPath));

            return GetProperyPathItems(math);
        }





        private static ProperyPath GetProperyPath(Match math)
        {
            return new ProperyPath()
            {
                Name = math.Groups["NAME"].Value,

                Items = GetProperyPathItems(math),

                Order = math.Groups["ORDER"].Value == "@"
                ? BindingSourceOrder.Ascending
                : BindingSourceOrder.Descending
            };
        }

        private static IReadOnlyList<ProperyPathItem> GetProperyPathItems(Match math)
        {
            var properyName = from i in math.Groups["NAME"].Captures.AsEnumerable() select new { i.Index, i.Value, IsIndex = false };
            var properyIndex = from i in math.Groups["INDEX"].Captures.AsEnumerable() select new { i.Index, i.Value, IsIndex = true };

            var properyList = properyName.Concat(properyIndex)
                .OrderBy(x => x.Index)
                .Select(x => new ProperyPathItem() { IsIndex = x.IsIndex, Value = x.Value })
                .ToArray();

            return properyList;
        }
    }
}
