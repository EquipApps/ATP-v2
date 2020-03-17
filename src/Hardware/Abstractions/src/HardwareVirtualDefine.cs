using System;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Определение виртуального устройства
    /// </summary>
    public class HardwareVirtualDefine
    {
        /// <summary>
        /// Имя виртуального устройства
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Поведения связанные с данным виртуальным устройство
        /// </summary>
        public Type[] BehaviorTypes { get; internal set; } 
    }
}
