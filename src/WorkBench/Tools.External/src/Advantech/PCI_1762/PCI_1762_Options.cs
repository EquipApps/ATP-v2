using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1762
{
    public class PCI_1762_Options
    {
        public PCI_1762_Options()
        {
            DllPath = "C:\\Windows\\AAPCtrlDev\\PCI_1762.dll";

            DeviceCollection = new List<PCI_1762_Device>();
        }

        /// <summary>
        /// Путь к DLL
        /// </summary>
        public string DllPath { get; set; }

        /// <summary>
        /// Коллекция <see cref="Psh3610_Device"/>
        /// </summary>
        public IList<PCI_1762_Device> DeviceCollection { get; }
    }
}
