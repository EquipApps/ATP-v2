using ReactiveUI;
using System;

namespace EquipApps.Mvc.Reactive
{
    public static class MessageBusEx
    {
        public static void SendFilterScope(string filterScope)
        {
            MessageBus.Current.SendMessage(filterScope, MessageBusContracts.FilterScope);
        }

        public static void SendEnabledBreakPoint(bool isEnabledBreakPoint)
        {
            MessageBus.Current.SendMessage(isEnabledBreakPoint, MessageBusContracts.EnabledBreakPoint);
        }
        public static void SendEnabledCheckPoint(bool isEnabledCheckPoint)
        {
            MessageBus.Current.SendMessage(isEnabledCheckPoint, MessageBusContracts.EnabledCheckPoint);
        }


        #region Listen

        public static IObservable<string> ListenFilterScope()
        {
            return MessageBus.Current.Listen<string>(MessageBusContracts.FilterScope);
        }

        public static IObservable<bool> ListenEnabledBreakPoint()
        {
            return MessageBus.Current.Listen<bool>(MessageBusContracts.EnabledBreakPoint);
        }
        public static IObservable<bool> ListenEnabledCheckPoint()
        {
            return MessageBus.Current.Listen<bool>(MessageBusContracts.EnabledCheckPoint);
        }

        #endregion
    }
}
