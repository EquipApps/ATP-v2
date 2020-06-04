using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Commutating
{
    public static class RelayBehaviorEx
    {
        public static void RelaySwitch(this IEnableContext enableContext, Relay relayState, string relayName)
        {
            enableContext.RequestToChangeValue<IRelayBehavior, Relay>(relayState, relayName);
        }
        public static void RelaySwitch(this IEnableContext enableContext, Relay relayState, params string[] relayNames)
        {
            enableContext.RequestToChangeValue<IRelayBehavior, Relay>(relayState, relayNames);
        }
        public static void RelayTransaction(this IEnableContext enableContext, Relay relayState, string relayName)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.RelaySwitch(relayState, relayName);

                transactionScope.Complete();
            }
        }
        public static void RelayTransaction(this IEnableContext enableContext, Relay relayState, params string[] relayNames)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.RelaySwitch(relayState, relayNames);

                transactionScope.Complete();
            }
        }
    }
}
