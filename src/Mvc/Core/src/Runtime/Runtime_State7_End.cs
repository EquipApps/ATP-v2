using EquipApps.Mvc.Runtime;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State7_End : IRuntimeState
    {
        public void Run(RuntimeContext context)
        {
            context.Handled = true;
        }
    }
}
