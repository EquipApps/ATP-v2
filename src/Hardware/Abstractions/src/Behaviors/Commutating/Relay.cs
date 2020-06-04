namespace EquipApps.Hardware.Behaviors.Commutating
{
    /// <summary>
    /// Состояния реле
    /// </summary>
    public enum Relay : byte
    {
        /// <summary>
        /// Open
        /// </summary>
        Disconnect = 0,

        /// <summary>
        /// Close
        /// </summary>
        Connect = 1,
    }
}
