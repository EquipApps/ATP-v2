using EquipApps.Mvc.Runtime;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State5_Move : IRuntimeState
    {
        public void Run(RuntimeContext context)
        {
            //
            // Переход к следующему шагу
            //
            if (context.Enumerator.MoveNext())
            {
                //
                // Переходим в состояние      
                //
                context.StateEnumerator.JumpTo(RuntimeStateType.INVOKE);
                context.StateEnumerator.MoveNext();
            }
            else
            {
                //
                // OБОШЛИ ВСЮ КОЛЛЕКЦИЮ!..
                //
                context.StateEnumerator.MoveNext();
            }
        }
    }
}