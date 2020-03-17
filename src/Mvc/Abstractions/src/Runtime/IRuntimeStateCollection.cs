namespace EquipApps.Mvc.Runtime
{
    public interface IRuntimeStateCollection
    {
        IRuntimeState this[int index] { get; }

        int Count { get; }

        int Index_end { get; }
        int Index_invoke { get; }
        int Index_move { get; }
        int Index_start { get; }


        IRuntimeStateEnumerator GetEnumerator();
    }
}
