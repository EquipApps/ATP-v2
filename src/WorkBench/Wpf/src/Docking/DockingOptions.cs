using System.Collections.Generic;

namespace EquipApps.WorkBench.Docking
{
    public class DockingOptions
    {
        public DockingOptions()
        {
            Files = new List<FileViewModel>();
            Tools = new List<ToolViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FileViewModel> Files { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ToolViewModel> Tools { get; }
    }
}
