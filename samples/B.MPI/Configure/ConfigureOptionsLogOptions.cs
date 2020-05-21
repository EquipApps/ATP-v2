using EquipApps.Mvc;
using EquipApps.Mvc.LogFeatures;
using EquipApps.Testing;
using Microsoft.Extensions.Options;

namespace B.MPI.Configure
{
    class ConfigureOptionsLogOptions : IConfigureOptions<LogOptions>
    {
        private const string gk_Test = "test";
        private const string gk_All = "all";

        public void Configure(LogOptions options)
        {
            options.GroupCollection.Add(gk_Test, new GroupInfo()
            {
                ShowManyContext = false,
                ShowNullContext = false,
                Title = "Краткий",
            });

            options.GroupCollection.Add(gk_All, new GroupInfo()
            {
                ShowManyContext = true,
                ShowNullContext = true,
                Title = "Полный",
            });


            options.AddContext<TestContext>(gk_Test);
        }
    }
}
