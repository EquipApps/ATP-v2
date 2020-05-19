namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610
{
    /// <summary>
    /// Устройство PSH-3610
    /// </summary>
    public class Psh3610_Device
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="number">Порядковый номер</param>
        /// <param name="comport">Номер COM</param>
        public Psh3610_Device(string name, ushort number, ushort comport)   
        {
            Name    = name;
            Number  = number;
            Port    = comport;
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public ushort Number { get; }

        /// <summary>
        /// Номер COM
        /// </summary>
        public ushort Port { get; }        
    }
}
