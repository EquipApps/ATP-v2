using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.Toggling;
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

            options.RegisterHardware<BatteryBehavior, ToggleBehavior>("ИП1");   //-- 3_В
            options.RegisterHardware<BatteryBehavior, ToggleBehavior>("ИП2");   //-- 30_В Шина +П



            /*-- Реальные устройства --*/

            options.RegisterMapping<PSP405,  PSxAdapter<PSP405>>  ("ИП1", () => new PSP405 (1,1));
            options.RegisterMapping<PSP2010, PSxAdapter<PSP2010>> ("ИП2", () => new PSP2010(1,2));
        }
    }
}
