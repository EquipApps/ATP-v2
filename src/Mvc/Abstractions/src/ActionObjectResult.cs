using System;

namespace EquipApps.Mvc
{
    public readonly struct ActionObjectResult
    {
        public ActionObjectResult(ActionObjectResultType type, Exception exception = null)
        {
            Type = type;
            Exception = exception;
        }

        /// <summary>
        /// Возвращает <see cref="ActionObjectResultType"/>
        /// </summary>
        public ActionObjectResultType Type { get; }

        /// <summary>
        /// Возвращает <see cref="Exception"/>
        /// </summary>
        public Exception Exception { get; }



        public bool IsEmpty => Type == ActionObjectResultType.NotRun && Exception == null;
    }


}
