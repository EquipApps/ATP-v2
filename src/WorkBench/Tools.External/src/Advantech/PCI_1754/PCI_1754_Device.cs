using EquipApps.Hardware;
using System;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1754
{
    /// <summary>
    /// <para>Advantech PCI-1754.</para>
    /// <para>Advantech PCI-1754.</para>  
    /// </summary>
    public class PCI_1754_Device
    {
        public PCI_1754_Device(ushort num, ushort board_ID)
        {
            this.Num      = num;
            this.Board_ID = board_ID;

            /*------------------------------------------------*/
            Ports = new PCI_DI_Port[8];
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new PCI_DI_Port((byte)(i + 1));
                Ports[i].ValueUpdate += PCI_1754_Device_ValueUpdate;
            }
            /*------------------------------------------------*/
        }

        private void PCI_1754_Device_ValueUpdate(ValueBehaviorBase<byte> behavior)
        {
            
        }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        public ushort Num { get; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public ushort Board_ID { get; }

        /// <summary>
        /// Цифровые порты
        /// </summary>
        public PCI_DI_Port[] Ports { get; }
    }
}
