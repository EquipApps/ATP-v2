using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Runtime;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime
{
    public class Runtime_State3_Pause : IRuntimeState, IRuntimeStatePause
    {
        private readonly ISubject<bool> _isPausedChangedSubject = new ReplaySubject<bool>();

        private volatile ManualResetEvent locker = new ManualResetEvent(false);
        private volatile bool _isEnabled = false;
        private volatile bool _isPaused = false;
        private WeakReference<RuntimeContext> weakContext = new WeakReference<RuntimeContext>(null);

        public void Run(RuntimeContext context)
        {
            if (IsEnabled)
            {
                //
                // Получаем текущий элемент ActionDescriptor
                //
                var descriptor = context.Enumerator.Current;
                if (descriptor == null)
                {
                    throw new ArgumentNullException(nameof(descriptor));
                }

                //-- Сохраняем в ленивую ссылку!
                weakContext.SetTarget(context);

                locker.Reset();

                descriptor.State = State.Pause;
                IsPaused = true;


                //-- Ожидаем --//


                locker.WaitOne();
                IsPaused = false;
                descriptor.State = State.Empy;
            }
            else
            {
                context.StateEnumerator.MoveNext();
            }
        }

        public void Next()
        {
            // Если конвеер не находиться в состояния ожидания, то не обрабатываем!
            if (!IsPaused) return;

            // Извлекаем контекст, изменяем состояние конвеера 
            if (weakContext.TryGetTarget(out RuntimeContext context))
            {
                context.StateEnumerator.MoveNext();

                weakContext.SetTarget(null);
                locker.Set();
            }
        }

        public void Replay()
        {
            // Если конвеер не находиться в состояния ожидания, то не обрабатываем!
            if (!IsPaused) return;

            // Извлекаем контекст, изменяем состояние конвеера 
            if (weakContext.TryGetTarget(out RuntimeContext context))
            {
                context.StateEnumerator.JumpTo(RuntimeStateType.INVOKE);
                context.StateEnumerator.MoveNext();

                weakContext.SetTarget(null);
                locker.Set();
            }
        }

        public void Previous()
        {
            // Если конвеер не находиться в состояния ожидания, то не обрабатываем!
            if (!IsPaused) return;

            // Извлекаем контекст, изменяем состояние конвеера 
            if (weakContext.TryGetTarget(out RuntimeContext context))
            {
                context.StateEnumerator.JumpTo(RuntimeStateType.INVOKE);

                weakContext.SetTarget(null);
                locker.Set();
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

        public bool IsPaused
        {
            get => _isPaused;
            private set
            {
                _isPaused = value;
                _isPausedChangedSubject.OnNext(value);
            }
        }

        #endregion

        #region Property [Observable]

        public IObservable<bool> IsPausedObservable => _isPausedChangedSubject.AsObservable();

        #endregion
    }
}
