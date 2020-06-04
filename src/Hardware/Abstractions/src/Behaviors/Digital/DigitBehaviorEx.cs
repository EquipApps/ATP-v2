using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EquipApps.Hardware.Behaviors.Digital
{
    public static class DigitBehaviorEx
    {
        public static void LineSwitch(this IEnableContext enableContext, Digit value, string line)
        {
            enableContext.RequestToChangeValue<IDigitBehavior, Digit>(value, line);
        }
        public static void LineSwitch(this IEnableContext enableContext, Digit value, params string[] lines)
        {
            enableContext.RequestToChangeValue<IDigitBehavior, Digit>(value, lines);
        }
        public static void LineTransaction(this IEnableContext enableContext, Digit value, string line)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.LineSwitch(value, line);

                transactionScope.Complete();
            }
        }
        public static void LineTransaction(this IEnableContext enableContext, Digit value, params string[] line)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.LineSwitch(value, line);

                transactionScope.Complete();
            }
        }

        public static Dictionary<string, Digit> LineRequest(this IEnableContext enableContext)
        {
            var feature = enableContext
                .TestContext
                .TestFeatures.Get<DigitFeature>();

            if (feature == null)
            {
                throw new InvalidOperationException(
                   string.Format("{0} не содержит {1}", nameof(TestContext), nameof(DigitFeature)));
            }

            using (var transactionScope = new TransactionScope())
            {
                foreach (var bihavior in feature.Values)
                {
                    bihavior.RequestToUpdateValue();
                }

                transactionScope.Complete();
            }

            var result = new Dictionary<string, Digit>();

            foreach (var pair in feature)
            {
                result[pair.Key] = pair.Value.Value;
            }


            return result;
        }
    }
}
