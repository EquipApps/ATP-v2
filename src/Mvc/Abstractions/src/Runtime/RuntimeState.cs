namespace EquipApps.Mvc.Runtime
{
    public enum RuntimeState : int
    {
        Reset = 0,
        Invoke = 1,
        Pause = 2,
        RepeatOnce = 3,
        Move = 4,
        Repeat = 5,
        End = 6,
    }
}
