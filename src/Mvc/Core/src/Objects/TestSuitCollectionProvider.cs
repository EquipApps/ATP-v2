using Microsoft.Extensions.Primitives;

namespace EquipApps.Mvc.Objects
{
    public abstract class TestSuitCollectionProvider
    {
        public abstract TestSuitCollection TestSuits { get; }

        public abstract IChangeToken GetChangeToken();
    }
}
