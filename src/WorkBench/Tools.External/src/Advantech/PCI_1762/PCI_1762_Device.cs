using EquipApps.Hardware;
using System;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1762
{
    public class PCI_1762_Device
    {
        public PCI_1762_Device(ushort num, ushort board_ID)
        {
            this.Num      = num;
            this.Board_ID = board_ID;           

            /*------------------------------------------------*/
            Relays = new PCI_1762_Relay[16];
            for (int i = 0; i < Relays.Length; i++)
            {
                Relays[i] = new PCI_1762_Relay((byte)(i + 1));
                Relays[i].ValueChange += PCI_1762_Device_ValueChange;
            }

            /*------------------------------------------------*/
            Ports = new PCI_DI_Port[2];
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new PCI_DI_Port((byte)(i + 1));
                Ports[i].ValueUpdate += PCI_1762_Device_ValueUpdate;
            }
        }

        private void PCI_1762_Device_ValueUpdate(ValueBehaviorBase<byte> behaviorBase)
        {
            throw new NotImplementedException();
        }

        private void PCI_1762_Device_ValueChange(ValueBehaviorBase<RelayState> behaviorBase, RelayState state)
        {
            throw new NotImplementedException();
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
        /// 
        /// </summary>
        public PCI_1762_Relay[]  Relays { get; }       

        public PCI_DI_Port[]   Ports { get; }
    }
}
