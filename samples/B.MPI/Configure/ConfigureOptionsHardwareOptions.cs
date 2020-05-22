using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using EquipApps.WorkBench;
using EquipApps.WorkBench.Tools.External.GwINSTEK;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_2010;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405;
using Microsoft.Extensions.Options;

namespace B.MPI.Configure
{
    public class ConfigureOptionsHardwareOptions : IConfigureOptions<HardwareOptions>
    {
        public void Configure(HardwareOptions options)
        {
            /*-- Виртуальные устройства --*/

            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП1");   //-- 3_В
            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП2");   //-- 30_В Шина +П



            /*-- Реальные устройства --*/

            options.RegisterMapping<Psp405_Device,  PS_Adapter<Psp405_Device>>  ("ИП1", () => new Psp405_Device (1,1));
            options.RegisterMapping<Psp2010_Device, PS_Adapter<Psp2010_Device>> ("ИП2", () => new Psp2010_Device(1,2));
        }
    }
}
