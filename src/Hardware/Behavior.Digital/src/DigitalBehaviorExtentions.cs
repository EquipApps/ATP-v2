using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EquipApps.Hardware
{
    public static class DigitalBehaviorExtentions
    {
        public static void DigitalSwitch(this IEnableContext enableContext, DigitalState digitalState, string digitalName)
        {
            enableContext.RequestToChangeValue<DigitalBehavior, DigitalState>(digitalState, digitalName);
        }
        public static void DigitalSwitch(this IEnableContext enableContext, DigitalState digitalState, params string[] digitalNames)
        {
            enableContext.RequestToChangeValue<DigitalBehavior, DigitalState>(digitalState, digitalNames);
        }
        public static void DigitalTransaction(this IEnableContext enableContext, DigitalState digitalState, string digitalName)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.DigitalSwitch(digitalState, digitalName);

                transactionScope.Complete();
            }
        }
        public static void DigitalTransaction(this IEnableContext enableContext, DigitalState digitalState, params string[] digitalNames)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.DigitalSwitch(digitalState, digitalNames);

                transactionScope.Complete();
            }
        }

        public static Dictionary<string, DigitalState> DigitalRequest(this IEnableContext enableContext)
        {
            var feature = enableContext
                .TestContext
                .TestFeatures.Get<DigitalFeature>();

            if (feature == null)
            {
                throw new InvalidOperationException(
                   string.Format("{0} не содержит {1}", nameof(TestContext), nameof(DigitalFeature)));
            }

            using (var transactionScope = new TransactionScope())
            {
                foreach (var bihavior in feature.Values)
                {
                    bihavior.RequestToUpdateValue();
                }

                transactionScope.Complete();
            }

            var result = new Dictionary<string, DigitalState>();

            foreach (var pair in feature)
            {
                result[pair.Key] = pair.Value.Value;
            }


            return result;
        }
    }
}
