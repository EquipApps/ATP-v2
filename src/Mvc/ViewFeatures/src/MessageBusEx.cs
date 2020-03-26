using ReactiveUI;
using System;

namespace EquipApps.Mvc
{
    public static class MessageBusEx
    {
        public static void SendEnabledBreakPoint(bool isEnabledBreakPoint)
        {
            MessageBus.Current.SendMessage<bool>(isEnabledBreakPoint, MessageBusContracts.EnabledBreakPoint);
        }
        public static void SendEnabledCheckPoint(bool isEnabledCheckPoint)
        {
            MessageBus.Current.SendMessage<bool>(isEnabledCheckPoint, MessageBusContracts.EnabledCheckPoint);
        }

        public static void SendFilterScope(string filterScope)
        {
            MessageBus.Current.SendMessage<string>(filterScope, MessageBusContracts.FilterScope);
        }

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
    }
}
