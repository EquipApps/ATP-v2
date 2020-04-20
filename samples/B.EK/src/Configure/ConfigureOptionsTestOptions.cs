using EquipApps.Testing;
using Microsoft.Extensions.Options;
using System;

namespace B.EK.Configure
{
    public class ConfigureOptionsTestOptions : IConfigureOptions<TestOptions>
    {
        public void Configure(TestOptions options)
        {
            options.ProductCode     = "ШЮГИ.";
            options.ProductName     = "Блок ПС";
            options.ProductVersion  = "Изм.0";
            options.Version         = new Version(1,0,0,0);

            //-- Устанавливаем режим работы по умолчанию
            options.SetWorkingMode  (Settings.WorkingMode_ZI);
            options.SetExecutingMode(Settings.ExecutingMode_Main);
            options.SetPowerMode    (Settings.PowerMode_NOM);
        }
    }
}
