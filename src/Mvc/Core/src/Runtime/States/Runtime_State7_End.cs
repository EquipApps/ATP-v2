using EquipApps.Mvc.Runtime;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State7_End : IRuntimeState
    {
        public void Handle(RuntimeContext context)
        {
            context.Handled = true;
        }
    }
}
