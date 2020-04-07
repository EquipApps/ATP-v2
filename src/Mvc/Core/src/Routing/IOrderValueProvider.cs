namespace EquipApps.Mvc.Routing
{
    public interface IOrderValueProvider
    {
        /// <summary>
        /// The order value key.
        /// </summary>
        string OrderKey { get; }

        /// <summary>
        /// The order value.
        /// </summary>
        string OrderValue { get; }
    }
}
