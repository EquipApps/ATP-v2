using System.Collections.Generic;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610
{
    /// <summary>
    /// Опции
    /// </summary>
    public class Psh3610_Options
    {
        public Psh3610_Options()
        {
            DllPath = "C:\\Windows\\AAPCtrlDev\\PSH_3610_GW_Instek.dll";

            DeviceCollection = new List<Psh3610_Device>();
        }

        /// <summary>
        /// Путь к DLL
        /// </summary>
        public string DllPath { get; set; }

        /// <summary>
        /// Коллекция <see cref="Psh3610_Device"/>
        /// </summary>
        public IList<Psh3610_Device> DeviceCollection { get; } 
    }
}
