using EquipApps.Mvc.Reactive.WorkFeatures;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    public class ConfigureOptionsRuntimeOptions : IConfigureOptions<RuntimeOptions>
    {
        public void Configure(RuntimeOptions options)
        {
            options.RepetTimeout = 0;       //-- Убираем задержку между повторами.
            options.RepetCount   = 100;     //-- 100 Циклов. (Значение согласно ТЗ)
        }
    }
}
