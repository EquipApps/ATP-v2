using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.PowerSource
{
    public static class PowerSourceBehaviorEx
    {
        /// <summary>
        /// Изменить состояние источника питания
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="state">Новое состояние источника питания</param>
        /// <param name="name">Имя источника питания</param>
        public static void PowerSourceSwitch(this IEnableContext enableContext, PowerSourceState state, string name)
        {
            enableContext.RequestToChangeValue<PowerSourceBehavior, PowerSourceState>(state, name);
        }

        /// <summary>
        /// Изменить состояние источников питания
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="state">Новое состояние источников питания</param>
        /// <param name="names">Имена источников питания</param>
        public static void PowerSourceSwitch(this IEnableContext enableContext, PowerSourceState state, string[] names)
        {
            enableContext.RequestToChangeValue<PowerSourceBehavior, PowerSourceState>(state, names);
        }

        /// <summary>
        /// <para>Изменить состояние источника питания</para>
        /// <para>C поддержкой транзакции</para>
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="state">Новое состояние источника питания</param>
        /// <param name="name">Имя источника питания</param>
        public static void PowerSourceTransaction(this IEnableContext enableContext, PowerSourceState state, string name)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.PowerSourceSwitch(state, name);

                transactionScope.Complete();
            }
        }

        /// <summary> 
        /// <para>Изменить состояние источников питания</para>
        /// <para>C поддержкой транзакции</para>
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="state">Новое состояние источников питания</param>
        /// <param name="names">Имена источников питания</param>
        public static void PowerSourceTransaction(this IEnableContext enableContext, PowerSourceState state, params string[] names)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.PowerSourceSwitch(state, names);

                transactionScope.Complete();
            }
        }
    }
}
