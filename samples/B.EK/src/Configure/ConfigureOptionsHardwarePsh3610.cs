using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    /// <summary>
    /// Конфигурация PSH-3610
    /// </summary>
    public class ConfigureOptionsHardwarePsh3610 : IConfigureOptions<Psh3610_Options>
    {
        public void Configure(Psh3610_Options options)
        {
            //-- Регистрация устройств

            options.DeviceCollection.Add(new Psh3610_Device("ИП1", 1, 1)
            {

            });

            options.DeviceCollection.Add(new Psh3610_Device("ИП2_+П", 2, 2)
            {

            });

            options.DeviceCollection.Add(new Psh3610_Device("ИП3_+С", 3, 3)
            {

            });

            options.DeviceCollection.Add(new Psh3610_Device("ИП4", 4, 4)
            {

            });
        }
    }
}
