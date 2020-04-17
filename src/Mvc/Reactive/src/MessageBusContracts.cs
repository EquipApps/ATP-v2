using ReactiveUI;

namespace EquipApps.Mvc.Reactive
{
    /// <summary>
    /// Контракты сообщений.
    /// Зарезервированные имена используются в <see cref="MessageBus"/>
    /// </summary>
    public static class MessageBusContracts
    {
        /// <summary>
        /// Фильтрация.
        /// </summary>
        public const string FilterScope = "FilterScope";

        /// <summary>
        /// Разрешить / запретить точки остановок
        /// </summary>
        public static string EnabledBreakPoint = "EnabledBreakPoint";

        /// <summary>
        /// Разрешить / запретить пропуски
        /// </summary>
        public static string EnabledCheckPoint = "EnabledCheckPoint";
    }
}
