using B.EK.Models;
using Microsoft.Extensions.Options;


namespace B.EK.Configure
{
    class ConfigureOptionsPowerOptions : IConfigureOptions<PowerOptions>
    {
        public void Configure(PowerOptions options)
        {
            options.Powers.Add("IP_6", new PowerOption()
            {
                Name       = "ИП(6В)",
                NomVoltage = 6.00,
                MinVoltage = 5.5,
                MaxVoltage = 6.5,
            });

            options.Powers.Add("IP_3", new PowerOption()
            {
                Name       = "ИП(3,3В)",
                NomVoltage = 3.3,
                MinVoltage = 3.0,
                MaxVoltage = 3.6,
            });

            options.Powers.Add("IP_C", new PowerOption()
            {
                Name       = "ИП(+С)",
                NomVoltage = 27,
                MinVoltage = 24,
                MaxVoltage = 32.5,
            });

            options.Powers.Add("IP_P", new PowerOption()
            {
                Name = "ИП(+P)",
                NomVoltage = 27,
                MinVoltage = 24,
                MaxVoltage = 32.5,
            });
        }
    }
}
