using System;

namespace EquipApps.Mvc.Abstractions
{
    public readonly struct ActionDescriptorResult
    {
        public ActionDescriptorResult(ActionDescriptorResultType type, Exception exception = null)
        {
            this.Type = type;
            this.Exception = exception;
        }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorResultType"/>
        /// </summary>
        public ActionDescriptorResultType Type { get; }

        /// <summary>
        /// Возвращает <see cref="Exception"/>
        /// </summary>
        public Exception Exception { get; }



        public bool IsEmpty => (Type == ActionDescriptorResultType.NotRun) && (Exception == null);
    }

    
}
