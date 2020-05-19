using EquipApps.WorkBench.Tools.External.Advantech.PCI_1762;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using Microsoft.Extensions.DependencyInjection;

namespace NLib.AtpNetCore.Testing
{
    public static class ToolExternalCollectionExtensions
    {
        public static void AddHardwareExternal(this IServiceCollection serviceCollection)
        {
            //-- GwINSTEK PSH-3610
            serviceCollection.AddTransient<Psh3610_Adapter>();
            serviceCollection.AddTransient<Psh3610_Library>();
            
            //-- Advantech PCI-1762
            serviceCollection.AddTransient<PCI_1762_Adapter>();
            serviceCollection.AddTransient<PCI_1762_Library>();
        }
    }
}
