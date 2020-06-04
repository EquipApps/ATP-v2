using System;

namespace EquipApps.Hardware.Abstractions
{
    /// <summary>
    /// Маркер поведения. С возможностью появления критичиских исключений
    /// </summary>
    public interface IUnhandledBehavior
    {
        /// <summary>
        /// Событие необработоного исключения.
        /// </summary>
        event UnhandledExceptionEventHandler UnhandledExceptionEvent;
    }
}