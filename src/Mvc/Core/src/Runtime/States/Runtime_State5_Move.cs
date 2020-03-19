using EquipApps.Mvc.Runtime;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State5_Move : IRuntimeState
    {
        public void Handle(RuntimeContext context)
        {
            //
            // Переход к следующему шагу
            //
            if (context.Action.MoveNext())
            {
                //
                // Переходим в состояние      
                //
                context.State.JumpTo(RuntimeStateType.INVOKE);
                context.State.MoveNext();
            }
            else
            {
                //
                // OБОШЛИ ВСЮ КОЛЛЕКЦИЮ!..
                //
                context.State.MoveNext();
            }
        }
    }
}