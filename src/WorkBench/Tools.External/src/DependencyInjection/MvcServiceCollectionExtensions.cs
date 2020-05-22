using EquipApps.WorkBench.Tools.External.Advantech.PCI_1762;
using EquipApps.WorkBench.Tools.External.GwINSTEK;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_2010;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSP_Series.PSP_405;
using Microsoft.Extensions.DependencyInjection;

namespace NLib.AtpNetCore.Testing
{
    public static class ToolExternalCollectionExtensions
    {
        public static void AddHardwareExternal(this IServiceCollection serviceCollection)
        {
            //-- GwINSTEK PSP-2010
            serviceCollection.AddTransient<PSxAdapter<PSP2010>>();

            //-- GwINSTEK PSP-405
            serviceCollection.AddTransient<PSxAdapter<PSP405>>();

            //-- GwINSTEK PSH-3610
            serviceCollection.AddTransient<PSxAdapter<PSH3610>>();







            
            
            //-- Advantech PCI-1762
            serviceCollection.AddTransient<PCI_1762_Adapter>();
            serviceCollection.AddTransient<PCI_1762_Library>();
        }
    }
}
