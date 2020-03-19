using EquipApps.Mvc.Runtime;
using System;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State1_Reset : IRuntimeState
    {
        public void Handle(RuntimeContext context)
        {
            //
            // Обновляем перечисление
            //
            context.Action.Reset();

            //
            // Проверка наличия ПЕРВОГО элемента
            //
            if (context.Action.MoveNext())
            {
                //
                // Переходим в следующее состояние      
                //
                context.State.MoveNext();

            }
            else
            {
                //
                // Первый элемент Null, что делать ?
                //
                throw new InvalidOperationException("Проверка прервана.Коллекция пуста!");
            }
        }
    }
}
