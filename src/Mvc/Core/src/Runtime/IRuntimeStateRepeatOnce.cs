
namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public interface IRuntimeStateRepeatOnce
    {
        bool IsEnabled { get; set; }

        int MillisecondsTimeout { get; set; }

    }
}