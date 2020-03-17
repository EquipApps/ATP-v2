using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    public class ActionDescriptorComparation : Comparer<ActionDescriptor>
    {
        public static int Comparison(ActionDescriptor x, ActionDescriptor y)
        {
            var result = x.TestCase.Number.CompareTo(y.TestCase.Number);

            if (result == 0)
            {
                result = x.TestStep.Number.CompareTo(y.TestStep.Number);
            }

            return result;
        }

        public override int Compare(ActionDescriptor x, ActionDescriptor y)
        {
            return Comparison(x, y);
        }
    }
}
