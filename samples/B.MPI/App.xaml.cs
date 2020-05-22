using B.MPI.Configure;
using EquipApps.Builder;
using EquipApps.Mvc.Reactive;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Testing;
using ReactiveUI;
using System.Reactive;
using System.Reflection;

namespace B.MPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : WbApp
    {
        public App()
        {
            Interactions.InteractionAcceptSettings.RegisterHandler(Handler);
        }

        private void Handler(InteractionContext<Unit, bool> interactionContext)
        {
            interactionContext.SetOutput(true);
        }

        protected override void Configure(ITestBuilder builder)
        {
            //-- Инициализация устройств
            builder.UseHardware();

            //-- Основная проверка
            builder.UseRuntime();
        }

        protected override void Configure(ILoggingBuilder builder)
        {
        }

        protected override void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHardwareExternal();

            serviceCollection.ConfigureOptions<ConfigureOptionsLogOptions>();
            serviceCollection.ConfigureOptions<ConfigureOptionsHardwareOptions>();
        }
    }
}
