using EquipApps.Mvc.Runtime;
using System.Threading;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public class Runtime_State6_Repeat : IRuntimeState, IRuntimeStateRepeat
    {
        private volatile bool _isEnabled = false;
        private volatile int _millisecondsTimeout = 100;

        public void Handle(RuntimeContext context)
        {
            if (IsEnabled)
            {
                context.State.JumpTo(RuntimeStateType.START);
                context.State.MoveNext();

                Thread.Sleep(MillisecondsTimeout);
            }
            else
            {
                context.State.MoveNext();
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
