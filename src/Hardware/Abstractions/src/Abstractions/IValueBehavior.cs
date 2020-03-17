using System;

namespace EquipApps.Hardware
{
    public interface IValueBehavior<TValue> : IHardwareBehavior
    {
        TValue Value { get; }

        void RequestToChangeValue(TValue value);
        void RequestToUpdateValue();
        void SetValue(TValue value);

        event Action<IValueBehavior<TValue>, TValue> ValueChange;
        event Action<IValueBehavior<TValue>> ValueUpdate;
    }
}