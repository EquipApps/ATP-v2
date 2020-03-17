using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationParts
{
    /// <summary>
    /// Часть приложения. Представляет собой <see cref="System.Reflection.Assembly"/>
    /// </summary>
    public class AssemblyPart : ApplicationPart
    {
        public AssemblyPart(Assembly assembly)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Возвращает <see cref="Assembly"/>
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Возвращает имя сборки
        /// </summary>
        public override string Name => Assembly.GetName().Name;

        /// <summary>
        /// Возвращает все типы в сборке
        /// </summary>
        public IEnumerable<TypeInfo> Types => Assembly.DefinedTypes;
    }
}
