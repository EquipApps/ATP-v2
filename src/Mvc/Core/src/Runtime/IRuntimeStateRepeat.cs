namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public interface IRuntimeStateRepeat
    {
        bool IsEnabled { get; set; }
        int MillisecondsTimeout { get; set; }
    }
}