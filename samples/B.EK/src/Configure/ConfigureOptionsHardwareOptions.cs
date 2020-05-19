using B.EK.ForDebug;
using EquipApps.Hardware;
using EquipApps.Hardware.Behaviors.PowerSource;
using EquipApps.WorkBench;
using EquipApps.WorkBench.Tools.External.Advantech.PCI_1762;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    public class ConfigureOptionsHardwareOptions : IConfigureOptions<HardwareOptions>
    {
        public void Configure(HardwareOptions options)
        {
            //-- Регистрация виртуальных устройств
            options.RegisterHardware<MeasureVoltageBehavior>("Multimetr");


            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП1");                //-- 3_В
            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП2_+П");             //-- 30_В Шина +П
            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП3_+С");             //-- 30_В Шина +С
            options.RegisterHardware<BatteryBehavior, PowerSourceBehavior>("ИП4");                //-- 6_В


            options.RegisterHardware<RelayBehavior>("K{0}", 50, 0);

            //-- Цифровая выдача
            options.RegisterHardware<DigitalBehavior>("W{0}.1", 16, 1);
            options.RegisterHardware<DigitalBehavior>("W{0}.2", 16, 1);
            options.RegisterHardware<DigitalBehavior>("W{0}.3", 16, 1);

            //-- Цифровой контроль
            options.RegisterHardware<DigitalBehavior>("R{0}.1", 16, 1);
            options.RegisterHardware<DigitalBehavior>("R{0}.2", 16, 1);
            options.RegisterHardware<DigitalBehavior>("R{0}.3", 16, 1);

            //-- Релейный контроль
            options.RegisterHardware<DigitalBehavior>("F{0}", 17, 1);
          
            //--
            

            options.RegisterMapping<Psh3610_Library,  Psh3610_Adapter>  ("PSH_3610");
            options.RegisterMapping<PCI_1762_Library, PCI_1762_Adapter> ("PCI_1762");

            options.RegisterMapping<ForDebugDevice, ForDebugDeviceAdapter>("SS");
        }
    }
}
