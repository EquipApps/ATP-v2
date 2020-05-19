using Microsoft.Extensions.Options;

namespace EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610
{
    /// <summary>
    /// PSH_3610_GW_Instek.dll
    /// </summary>
    public class Psh3610_Library : PshLibrary
    {
        public Psh3610_Library(IOptions<Psh3610_Options> options)
            : base(options?.Value?.DllPath)
        {
            //InitializeComponent();
        }
    }
}
