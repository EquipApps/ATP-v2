using EquipApps.Mvc.Runtime;
using System;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class Runtime_State1_Reset : IRuntimeState
    {
        public void Run(RuntimeContext context)
        {
            //
            // Обновляем перечисление
            //
            context.Enumerator.Reset();

            //
            // Проверка наличия ПЕРВОГО элемента
            //
            if (context.Enumerator.MoveNext())
            {
                //
                // Переходим в следующее состояние      
                //
                context.StateEnumerator.MoveNext();

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
