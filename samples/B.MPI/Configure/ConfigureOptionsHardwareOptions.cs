using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using EquipApps.WorkBench;
using EquipApps.WorkBench.Tools.External.Advantech.PCI_1762;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using Microsoft.Extensions.Options;

namespace B.MPI.Configure
{
    public class ConfigureOptionsHardwareOptions : IConfigureOptions<HardwareOptions>
    {
        public void Configure(HardwareOptions options)
        {

            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП1");   //-- 3_В
            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП2");   //-- 30_В Шина +П
           



            options.RegisterMapping<Psh3610_Library, Psh3610_Adapter>("PSH_3610");
            options.RegisterMapping<PCI_1762_Library, PCI_1762_Adapter>("PCI_1762");
        }
    }
}
