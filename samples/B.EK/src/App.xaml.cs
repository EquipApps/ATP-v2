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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Testing;
using Serilog;
using Serilog.Filters;
using Splat;
using System.Reflection;

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
                .Filter.ByIncludingOnly(Matching.FromSource(typeof(TestContext).FullName))
                .WriteTo.File("Log.txt")
                .CreateLogger();

            return log;
        }

        protected override void Configure(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddSerilog(Logger);           
        }

        protected override void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            

            serviceCollection.AddTransient<IMahAppsService, MahAppsService>();

            serviceCollection.AddSingleton<OptionsViewModel>();
            


            serviceCollection.AddTransient<ForDebugDevice>();
            serviceCollection.AddTransient<ForDebugDeviceAdapter>();

            serviceCollection.ConfigureOptions<ConfigureOptionsHardwareOptions>();
            serviceCollection.ConfigureOptions<ConfigureOptionsLogOptions>();
            serviceCollection.ConfigureOptions<ConfigureOptionsTestOptions>();           
            serviceCollection.ConfigureOptions<ConfigureOptionsRuntimeOptions>();

            serviceCollection.AddTransientMvcFeatureConvetion<MvcFeatureConvetion>();

            //-- MVC
            serviceCollection.AddMvc();
          
            serviceCollection.AddMvcAssemply(Assembly.GetEntryAssembly());
            

            serviceCollection.AddMvcModelProvider<Command, CommandProvider>();


            //-- Hardware
            serviceCollection.AddHardware();
            serviceCollection.AddHardwareDigital();


            //-- WB

        }

        protected override void  Configure(ITestBuilder builder)
        {
            //--
            builder.UseUnhandledException((context, ex) =>
            {
                context.TestLogger.LogCritical(ex, "Необработанное исключение");
                throw ex;
            });

            builder.Use((main) =>
            {
                return async context =>
                {
                    try
                    {
                       
                        //-- Тут мы создаем новый протокол
                        var logManager = context.TestServices.GetService<ILogManager>();

                        await main(context);
                    }
                    finally
                    {
                        //-- Тут мы сохряняем протокол
                        Log.CloseAndFlush();
                    }
                };
            });

            //-- Очистка области протокола.
            builder.UseLogsClean();

            //-- Обновление состояний проверки.
            builder.UseViewUpdate();

            builder.Use((TestContext context) =>
            {
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
            builder.UseRuntime();

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
