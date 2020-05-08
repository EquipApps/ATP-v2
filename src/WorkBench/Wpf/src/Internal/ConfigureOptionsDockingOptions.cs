using EquipApps.WorkBench.Docking;
using EquipApps.WorkBench.DockingErrorList;
using EquipApps.WorkBench.DockingTestExplorer;
using EquipApps.WorkBench.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace EquipApps.WorkBench.Internal
{
    public class ConfigureOptionsDockingOptions : IConfigureOptions<DockingOptions>
    {
        IServiceProvider _serviceProvider;

        public ConfigureOptionsDockingOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Configure(DockingOptions options)
        {
            options.Files.Add(_serviceProvider.GetService<TestExplorerViewModel>());

            options.Tools.Add(_serviceProvider.GetService<WorkViewerViewModel>());
            options.Tools.Add(_serviceProvider.GetService<ErrorListViewModel>());
        }
    }
}
