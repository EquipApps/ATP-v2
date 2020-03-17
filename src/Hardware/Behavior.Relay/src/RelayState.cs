namespace EquipApps.Hardware
{
    /// <summary>
    /// Состояния реле
    /// </summary>
    public enum RelayState : byte
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
