using System;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public interface IRuntimeStatePause
    {
        bool IsEnabled { get; set; }
        bool IsPaused { get; }
        IObservable<bool> IsPausedObservable { get; }

        void Next();
        void Replay();
        void Previous();
    }
}