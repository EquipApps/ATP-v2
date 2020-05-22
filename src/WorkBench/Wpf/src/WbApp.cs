using EquipApps.Testing.Wpf;
using EquipApps.WorkBench.Docking;
using EquipApps.WorkBench.DockingErrorList;
using EquipApps.WorkBench.DockingTestExplorer;
using EquipApps.WorkBench.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EquipApps.WorkBench
{
    public abstract partial class WbApp : TestApplication
    {
        partial void InitializeResources()
        {
            var internalRes = new WbAppResource();

            this.Resources.MergedDictionaries.Add(internalRes);
        }

        partial void ConfigureServiceForShell(IServiceCollection serviceCollection)
        {
            //-------------------------------------------------------------
            //-- Регистрация Док контейнера
            serviceCollection.AddSingleton<DockingViewModel>();

            //-- Оболочка
            serviceCollection.AddTransient<IShell, WbShell>();
            
            //-------------------------------------------------------------
            //-- Регистрация Инструментов

            serviceCollection.AddTransient<TestExplorerViewModel>   ();
            serviceCollection.AddTransient<ErrorListViewModel>      ();

            //-- Конфигурация Docking
            serviceCollection.ConfigureOptions<ConfigureOptionsDockingOptions>();
        }
    }
}
