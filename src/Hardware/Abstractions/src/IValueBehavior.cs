using System;

namespace EquipApps.Hardware
{
    public interface IValueBehavior<TValue> : IHardwareBehavior
    {
        /// <summary>
        /// Value
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Request To Change Value
        /// </summary>  
        void RequestToChangeValue(TValue value);

        /// <summary>
        /// Request To Update Value.
        /// </summary>
        void RequestToUpdateValue();
    }
}