using B.EK.Configure;
using B.EK.Data;
using B.EK.ForDebug;
using B.EK.Models;
using B.EK.Services;
using B.EK.ViewModels;
using EquipApps.Builder;
using EquipApps.Testing;
using EquipApps.WorkBench;
using EquipApps.WorkBench.Services;
using EquipApps.WorkBench.ViewModels;
using EquipApps.WorkBench.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Builder;
using NLib.AtpNetCore.Testing;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reflection;
using System.Windows;

namespace B.EK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : WbApp
    {
        public Serilog.ILogger Logger { get; }

        public App()
        {
            Logger = CreateLogger();
        }

        private Serilog.ILogger CreateLogger()
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()//outputTemplate: "[{Properties}{NewLine}{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            return log;
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var ttt = this.ServiceProvider.GetServices<EquipApps.Testing.Features.IFeatureProvider>();

            var vm1 = this.ServiceProvider.GetRequiredService<LogViewerViewModel>();
            var vm2 = this.ServiceProvider.GetRequiredService<TestViewerViewModel>();
            var vm3 = this.ServiceProvider.GetRequiredService<WorkViewerViewModel>();


            var vm4 = this.ServiceProvider.GetRequiredService<ActionsByResultTool>();

            Workspace.This.Tools.Add(vm1);
            Workspace.This.Tools.Add(vm3);
            Workspace.This.Tools.Add(vm4);


            Workspace.This.Files.Add(vm2);


            var shell = new ShellWindow();
                shell.Show();
        }

        protected override void Configure(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

            loggingBuilder.AddSerilog(Logger);
            loggingBuilder.Services.AddSingleton<ILoggerProvider, LogEntryLoggerProvider>();

        }

        protected override void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(typeof(LogViewerViewModel).Assembly);

            serviceCollection.AddTransient<IMahAppsService, MahAppsService>();

            serviceCollection.AddSingleton<OptionsViewModel>();

            serviceCollection.AddTransient<ForDebugDevice>();
            serviceCollection.AddTransient<ForDebugDeviceAdapter>();

            serviceCollection.ConfigureOptions<ConfigureOptionsHardwareOptions>();
            serviceCollection.ConfigureOptions<ConfigureOptionsLogOptions>();
            serviceCollection.ConfigureOptions<ConfigureOptionsTestOptions>();


            //-- MVC
            serviceCollection.AddMvc();
            serviceCollection.AddMvcAssemply(Assembly.GetEntryAssembly());
            serviceCollection.AddActionDescriptorProvider<ConfigureActions>();

            serviceCollection.AddMvcModelProvider<Command, CommandProvider>();


            //-- Hardware
            serviceCollection.AddHardware();
            serviceCollection.AddHardwareDigital();


            //-- WB
            serviceCollection.AddWb();

        }

        protected override void  Configure(ITestBuilder builder)
        {
            //--
            builder.UseUnhandledException((context, ex) =>
            {
                context.TestLogger.LogCritical(ex, "Необработанное исключение");
                throw ex;
            });
           
            builder.Use(async (TestContext context) =>
            {
                //-- Очищаем список ошибок
                var logService = context.TestServices.GetService<ILogEntryService>();
                await logService.CleanAsync();

                //-- Выводим информацию в протокол
                context.TestLogger.LogInformation("Проверка");
                context.TestLogger.LogInformation(context.TestOptions.ProductName);
                context.TestLogger.LogInformation(context.TestOptions.ProductCode);
                context.TestLogger.LogInformation(context.TestOptions.ProductVersion);

                context.TestLogger.LogInformation(context.TestOptions.GetWorkingMode<string>());
                context.TestLogger.LogInformation(context.TestOptions.GetExecutingMode<string>());
                context.TestLogger.LogInformation(context.TestOptions.GetPowerMode<string>());
            });

            //-- Инициализация устройств
            builder.UseHardware();

            //-- Основная проверка
            builder.UseMvc();

            //-- Выводим информацию в протокол
            builder.Use((TestContext context) =>
            {
                if (context.TestAborted.IsCancellationRequested)
                {
                    context.TestLogger.LogInformation("Проверка прервана");
                }
                else
                {
                    context.TestLogger.LogInformation("Проверка завершена");
                }
            });
        }
    }
}
