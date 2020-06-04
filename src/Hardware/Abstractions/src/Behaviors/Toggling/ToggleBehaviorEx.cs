using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Toggling
{
    /// <summary>
    /// Расширение. Позволяет легко управлять тумблерами.
    /// </summary>
    /// 
    /// <remarks>
    /// Важно!. Не реализовывать функции переключения сразу всех тумблеров. т.к можно по ошибке чтонибудь сжечь 
    /// </remarks>
    /// 
    public static class ToggleBehaviorEx
    {
        /// <summary>
        /// Изменить состояние тумблера
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="toggle">Новое Состояние переключателя</param>
        /// <param name="toggleName">Имя переключателя</param>
        public static void ToggleSwitch(this IEnableContext enableContext, Toggle toggle, string toggleName)
        {
            enableContext.RequestToChangeValue<IToggleBehavior, Toggle>(toggle, toggleName);
        }

        /// <summary>
        /// Изменить состояние тумблера
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="toggle">Новое состояние переключателя</param>
        /// <param name="toggleNames">Имена переключателей</param>
        public static void ToggleSwitch(this IEnableContext enableContext, Toggle toggle, string[] toggleNames)
        {
            enableContext.RequestToChangeValue<IToggleBehavior, Toggle>(toggle, toggleNames);
        }

        /// <summary>
        /// <para>Изменить состояние тумблера</para>
        /// <para>C поддержкой транзакции</para>
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="toggle">Новое состояние переключателя</param>
        /// <param name="toggleName">Имя переключателя</param>
        public static void ToggleTransaction(this IEnableContext enableContext, Toggle toggle, string toggleName)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.ToggleSwitch(toggle, toggleName);

                transactionScope.Complete();
            }
        }

        /// <summary> 
        /// <para>Изменить состояние тумблера</para>
        /// <para>C поддержкой транзакции</para>
        /// </summary>
        /// <param name="enableContext"></param>
        /// <param name="toggle">Новое состояние переключателя</param>
        /// <param name="toggleNames">Имена переключателей</param>
        public static void ToggleTransaction(this IEnableContext enableContext, Toggle toggle, params string[] toggleNames)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.ToggleSwitch(toggle, toggleNames);

                transactionScope.Complete();
            }
        }
    }
}
