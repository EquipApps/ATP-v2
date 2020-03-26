using EquipApps.Mvc;
using EquipApps.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.Controllers
{
    /// <summary>
    /// Провайдер <see cref="ControllerFeature"/>
    /// </summary>
    public class ControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private const string ControllerTypeNameSuffix = "Controller";

        /// <summary>
        /// Наполняет <see cref="ControllerFeature"/> используя <see cref="AssemblyPart"/>
        /// </summary>
        /// 
        /// <param name="parts">
        /// Перечисление модулей приложения
        /// </param>
        /// 
        /// <param name="feature">
        /// Расширение
        /// </param>
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var part in parts.OfType<AssemblyPart>())
            {
                foreach (var type in part.Types)
                {
                    if (IsController(type) && !feature.Controllers.Contains(type))
                    {
                        feature.Controllers.Add(type);
                    }
                }
            }
        }

        /// <summary>
        /// Логика валидации типа, как контроллера
        /// </summary>        
        protected virtual bool IsController(TypeInfo typeInfo)
        {
            //-- Только класс
            if (!typeInfo.IsClass)
            {
                return false;
            }

            //-- Не абстрактный класс
            if (typeInfo.IsAbstract)
            {
                return false;
            }

            //-- Публичный
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            //-- Не генерик тип!
            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            //-- 
            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }
    }
}
