using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    public class ActionDescriptorComparation : Comparer<ActionDescriptor>
    {
        public static int Comparison(ActionDescriptor x, ActionDescriptor y)
        {
            return x.Number.CompareTo(y.Number);
        }

        public override int Compare(ActionDescriptor x, ActionDescriptor y)
        {
            return Comparison(x, y);
        }
    }
}
