using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using System;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK
{
    /// <summary>
    /// <para>Адаптер источников питания</para>
    /// <para>PSP-2010; PSP-405;</para>
    /// </summary>
    /// <typeparam name="TPowerSource"></typeparam>
    public class PSxAdapter<TPowerSource> : HardwareAdapterBase<TPowerSource>
        where TPowerSource : class, IPowerSource
    {
        private TPowerSource _device;
        private string       _deviceName;


        protected override void Adapt(TPowerSource device, string deviceName)
        {
            _device     = device;
            _deviceName = deviceName;
        }

        protected override void AttachBehaviors()
        {
            //-- Извлекаем виртуальное устройство по имени
            var hardware = this.HardwareFeature.HardwareCollection[_deviceName];
            if (hardware == null)
            {
                //TODO: Добавит лог.
                return;
            }

            var behavior = new PowerSourceBehavior();
                behavior.ValueChange += Behavior_ValueChange;

            //-- Сохпвняем поведение
            hardware.Behaviors.AddOrUpdate(behavior);
        }

        private void Behavior_ValueChange(ValueBehaviorBase<PowerSourceState> behaviorBase, PowerSourceState sourceState)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            (_device as IDisposable)?.Dispose();
             _device = null;
        }
    }
}
