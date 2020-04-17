namespace EquipApps.Mvc.Runtime
{
    public class RepeatModule
    {
        private readonly object locker = new object();
        
        private volatile bool _isEnabled;
        private volatile int  _count;

        public RepeatModule()
        {

        }

        public void Enabled(bool isEnabled, int count = -1)
        {
            lock (locker)
            {
                _isEnabled = isEnabled;
                _count     = isEnabled ? count : -1;
            }
        }
        public bool TryRepeat()
        {
            bool isEnabled = false;
            int count = -1;

            lock (locker)
            {
                isEnabled = _isEnabled;
                count     = _count;
            }

            return false;
        }


        private struct State
        {
            bool IsEnabled;
            int  Count;
        }
    }
}
