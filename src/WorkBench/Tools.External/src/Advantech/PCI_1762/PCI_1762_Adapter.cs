using EquipApps.Hardware;
using Microsoft.Extensions.Options;
using System;

namespace EquipApps.WorkBench.Tools.External.Advantech.PCI_1762
{
    public class PCI_1762_Adapter : HardwareAdapterBase<PCI_1762_Library>
    {
        private readonly PCI_1762_Options options;
        private PCI_1762_Library library;

        public PCI_1762_Adapter(IOptions<PCI_1762_Options> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override void Adapt(PCI_1762_Library device, string deviceName)
        {
            library = device ?? throw new ArgumentNullException(nameof(device));
        }

        protected override void AttachBehaviors()
        {
            foreach (var device in options.DeviceCollection)
            {
                foreach (var relay in device.Relays)
                {
                    //-- Извлекаем виртуальное устройство по имени
                    var hardware =  HardwareFeature.HardwareCollection[relay.Name];
                    if (hardware == null)
                    {
                        //TODO: Добавит лог.
                        continue;
                    }

                    hardware.Behaviors.AddOrUpdate<IRelayBehavior>(relay);
                }

            }
        }



















        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        
    }
}
