using EquipApps.Mvc;
using EquipApps.Testing;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    class ConfigureOptionsLogOptions : IConfigureOptions<LogOptions>
    {
        private const string gk_Test = "test";
        private const string gk_All = "all";

        public void Configure(LogOptions options)
        {
            options.GroupCollection.Add(gk_All, new GroupInfo()
            {
                ShowManyContext = true,
                ShowNullContext = true,
                Title = "Отладка",
            });

            options.GroupCollection.Add(gk_Test, new GroupInfo()
            {
                ShowManyContext = false,
                ShowNullContext = false,
                Title = "Проверка",
            });


            options.AddContext<TestContext>(gk_Test);
        }
    }
}
