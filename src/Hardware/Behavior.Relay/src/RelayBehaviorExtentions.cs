using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EquipApps.Hardware
{
    public static class RelayBehaviorExtentions
    {
        public static void RelaySwitch(this IEnableContext enableContext, RelayState relayState, string relayName)
        {
            enableContext.RequestToChangeValue<RelayBehavior, RelayState>(relayState, relayName);
        }
        public static void RelaySwitch(this IEnableContext enableContext, RelayState relayState, params string[] relayNames)
        {
            enableContext.RequestToChangeValue<RelayBehavior, RelayState>(relayState, relayNames);
        }
        public static void RelayTransaction(this IEnableContext enableContext, RelayState relayState, string relayName)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.RelaySwitch(relayState, relayName);

                transactionScope.Complete();
            }
        }
        public static void RelayTransaction(this IEnableContext enableContext, RelayState relayState, params string[] relayNames)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.RelaySwitch(relayState, relayNames);

                transactionScope.Complete();
            }
        }
    }
}
