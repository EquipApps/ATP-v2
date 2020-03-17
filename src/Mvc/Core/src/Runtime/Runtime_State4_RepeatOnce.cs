using EquipApps.Mvc.Runtime;
using System.Threading;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public class Runtime_State4_RepeatOnce : IRuntimeState, IRuntimeStateRepeatOnce
    {
        private volatile bool _isEnabled = false;
        private volatile int _millisecondsTimeout = 100;

        public void Run(RuntimeContext context)
        {
            if (IsEnabled)
            {
                context.StateEnumerator.JumpTo(RuntimeStateType.INVOKE);
                context.StateEnumerator.MoveNext();

                Thread.Sleep(MillisecondsTimeout);
            }
            else
            {
                context.StateEnumerator.MoveNext();
            }
        }


        #region Property

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
            }
        }

        public int MillisecondsTimeout
        {
            get => _millisecondsTimeout;
            set
            {
                _millisecondsTimeout = value;
            }
        }

        #endregion
    }
}
