﻿using B.EK.ForDebug;
using EquipApps.Hardware;
using EquipApps.WorkBench;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    public class ConfigureOptionsHardwareOptions : IConfigureOptions<HardwareOptions>
    {
        public void Configure(HardwareOptions options)
        {
            //-- Регистрация виртуальных устройств
            options.RegisterHardware<MeasureVoltageBehavior>("Multimetr");


            options.RegisterHardware<BatteryBehavior>("ИП1");                //-- 3_В
            options.RegisterHardware<BatteryBehavior>("ИП2_+П");             //-- 30_В Шина +П
            options.RegisterHardware<BatteryBehavior>("ИП3_+С");             //-- 30_В Шина +С
            options.RegisterHardware<BatteryBehavior>("ИП4");                //-- 6_В


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
          

            options.RegisterMapping<ForDebugDevice, ForDebugDeviceAdapter>("DEBUG_DEVICE");
        }
    }
}
