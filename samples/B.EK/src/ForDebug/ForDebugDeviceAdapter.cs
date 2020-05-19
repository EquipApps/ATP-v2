using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using EquipApps.WorkBench;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace B.EK.ForDebug
{
    public class ForDebugDeviceAdapter : HardwareAdapterBase<ForDebugDevice>
    {
        private ForDebugDevice device;
        private string deviceName;
        private ILogger<ForDebugDeviceAdapter> logger;
        private HardwareOptions hardwareOptions;

        public ForDebugDeviceAdapter(
            ILogger<ForDebugDeviceAdapter> logger,
            IOptions<HardwareOptions> hardwareOptions)
        {
            this.logger = logger ?? throw new ArgumentException(nameof(logger));
            this.hardwareOptions = hardwareOptions?.Value ?? throw new ArgumentException(nameof(hardwareOptions));
        }











        protected override void Adapt(ForDebugDevice device, string deviceName)
        {
            logger.LogTrace($"{deviceName} - Adapt");

            this.device = device;
            this.deviceName = deviceName;
        }

        protected override void AttachBehaviors()
        {
            logger.LogTrace($"{deviceName} - AttachBehaviors");

            RegisterBatteryBehavior();
            RegisterDigitalBehavior();
            RegisterMeasureVoltageBehavior();


            RegisterRelayBehavior();
        }



        private void RegisterBatteryBehavior()
        {
            logger.LogTrace($"{deviceName} - RegisterBatteryBehavior");

            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(BatteryBehavior))))
            {
                var hardware = this.HardwareFeature.HardwareCollection[virtualDefine.Name];
                var behavior = new BatteryBehavior();

                hardware.Behaviors.AddOrUpdate(behavior);
            }
        }
        private void RegisterDigitalBehavior()
        {
            logger.LogTrace($"{deviceName} - RegisterDigitalBehavior");

            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(DigitalBehavior))))
            {
                var hardware = this.HardwareFeature.HardwareCollection[virtualDefine.Name];

                var behavior = new DigitalBehavior();
                behavior.SetValue(DigitalState.Zed);

                if (virtualDefine.Name.FirstOrDefault() == 'W')
                {
                    behavior.ValueChange += DigitalBehavior_ValueChange;
                }
                else
                {
                    behavior.ValueUpdate += DigitalBehavior_ValueUpdate;
                }



                hardware.Behaviors.AddOrUpdate(behavior);
            }
        }
        private void RegisterRelayBehavior()
        {
            logger.LogTrace($"{deviceName} - RegisterRelayBehavior");

            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(IRelayBehavior))))
            {
                var hardware = this.HardwareFeature.HardwareCollection[virtualDefine.Name];
                if (hardware.Behaviors.ContainsBehaviorWithKey<IRelayBehavior>())
                    continue;

                var behavior = new RelayBehavior();
                behavior.SetValue(RelayState.Disconnect);
                behavior.ValueChange += RelayBehavior_ValueChange;

                hardware.Behaviors.AddOrUpdate<IRelayBehavior>(behavior);
            }
        }
        private void RegisterMeasureVoltageBehavior()
        {
            logger.LogTrace($"{deviceName} - MeasureVoltageBehavior");

            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(MeasureVoltageBehavior))))
            {
                var hardware = this.HardwareFeature.HardwareCollection[virtualDefine.Name];

                var behavior = new MeasureVoltageBehavior();
                behavior.ValueUpdate += MeasureVoltageBehavior_ValueUpdate;
                hardware.Behaviors.AddOrUpdate(behavior);
            }
        }
        private void RegisterPowerSourceBehavior()
        {
            logger.LogTrace($"{deviceName} - RegisterPowerSourceBehavior");

            foreach (var virtualDefine in hardwareOptions.VirtualDefines.Where(x => x.BehaviorTypes.Contains(typeof(PowerSourceBehavior))))
            {
                var hardware = this.HardwareFeature.HardwareCollection[virtualDefine.Name];
                var behavior = new PowerSourceBehavior();
                behavior.ValueChange += PowerSourceBehavior_ValueChange;


                hardware.Behaviors.AddOrUpdate(behavior);
            }
        }

        private void PowerSourceBehavior_ValueChange(ValueBehaviorBase<PowerSourceState> behavior, PowerSourceState value)
        {
            logger.LogTrace("{device}:{hardware} - {value}", deviceName, behavior.Hardware.Name, value);
            behavior.SetValue(value);
        }
        private void DigitalBehavior_ValueChange(ValueBehaviorBase<DigitalState> behavior, DigitalState value)
        {
            logger.LogTrace("{device}:{hardware} - {value}", deviceName, behavior.Hardware.Name, value);
            behavior.SetValue(value);
        }
        private void DigitalBehavior_ValueUpdate(ValueBehaviorBase<DigitalState> behavior)
        {
            logger.LogTrace("{device}:{hardware} - Update", deviceName, behavior.Hardware.Name);

            behavior.SetValue(DigitalState.Null);
        }

        private void MeasureVoltageBehavior_ValueUpdate(ValueBehaviorBase<double> behavior)
        {
            logger.LogTrace("{device}:{hardware} - Measure Voltage", deviceName, behavior.Hardware.Name);

            behavior.SetValue(0);
        }
        private void RelayBehavior_ValueChange(ValueBehaviorBase<RelayState> behavior, RelayState value)
        {
            logger.LogTrace("{device}:{hardware} - {value}", deviceName, behavior.Hardware.Name, value);

            behavior.SetValue(value);
        }




        protected override void InitializeDevice()
        {
            logger.LogTrace($"{deviceName} - Initialize");
        }

        protected override void ResetDevice()
        {
            logger.LogTrace($"{deviceName} - Reset");
        }

        public override void Dispose()
        {
            logger.LogTrace($"{deviceName} - Dispose");
        }


    }
}
