using EquipApps.Hardware.Extensions;
using EquipApps.Testing;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EquipApps.Hardware
{
    public static class DigitalBehaviorExtentions
    {
        public static void LineSwitch(this IEnableContext enableContext, byte value, string line)
        {
            enableContext.RequestToChangeValue<IDigitalLineBehavior, byte>(value, line);
        }
        public static void LineSwitch(this IEnableContext enableContext, byte value, params string[] lines)
        {
            enableContext.RequestToChangeValue<IDigitalLineBehavior, byte>(value, lines);
        }
        public static void LineTransaction(this IEnableContext enableContext, byte value, string line)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.LineSwitch(value, line);

                transactionScope.Complete();
            }
        }
        public static void LineTransaction(this IEnableContext enableContext, byte value, params string[] line)
        {
            using (var transactionScope = new TransactionScope())
            {
                enableContext.LineSwitch(value, line);

                transactionScope.Complete();
            }
        }

        public static Dictionary<string, byte> LineRequest(this IEnableContext enableContext)
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

            var result = new Dictionary<string, byte>();

            foreach (var pair in feature)
            {
                result[pair.Key] = pair.Value.Value;
            }


            return result;
        }
    }
}
