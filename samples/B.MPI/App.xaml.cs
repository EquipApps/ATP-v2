using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace B.MPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : WbApp
    {
        protected override void Configure(ITestBuilder builder)
        {
        }

        protected override void Configure(ILoggingBuilder builder)
        {
        }

        protected override void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            
        }
    }
}
