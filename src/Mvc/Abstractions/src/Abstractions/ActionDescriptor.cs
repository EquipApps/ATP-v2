using EquipApps.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EquipApps.Mvc
{
    public delegate void ActionDescriptorUpdate(ActionDescriptor action);

    /// <summary>
    /// Дескриптер действия
    /// </summary>
    public abstract partial class ActionDescriptor : IDisposable
    {
        private readonly ISubject<bool> _checkChangedSubject = new ReplaySubject<bool>();
        private readonly ISubject<bool> _breakChangedSubject = new ReplaySubject<bool>();

        private readonly ISubject<Exception> _exceptionChangedSubject = new ReplaySubject<Exception>();
        private readonly ISubject<Result> _resultChangedSubject = new ReplaySubject<Result>();
        private readonly ISubject<State> _stateChangedSubject = new ReplaySubject<State>();

        private Exception _exception;
        private Result _result;
        private State _state;

        private static int Count = 0;

        public ActionDescriptor(ActionDescriptorObject testCase, ActionDescriptorObject testStep)
            :this()
        {
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
            TestStep = testStep ?? throw new ArgumentNullException(nameof(testStep));

            Count++;
            Exception = null;
            Result = Result.NotExecuted;
            State  = State.Empy;
        }

      

        




        //-------------------------------

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого случая
        /// </summary>
        public virtual ActionDescriptorObject TestCase { get; }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого шага
        /// </summary>
        public virtual ActionDescriptorObject TestStep { get; }

        //-------------------------------

        [Obsolete("User RouteValues")]
        public string Area
        {
            get
            {
                if (RouteValues.TryGetValue("Area", out string value))
                    return value;
                else
                    return null;
            }
        }
        public virtual Number Number { get; set; }
       

        #region Property

        /// <summary>
        /// Флаг. Указывает будет ли элемент участвовать в проверке.
        /// </summary>
        public bool IsCheck { get; set; } = true;

        /// <summary>
        /// Флаг. Точки остановки
        /// </summary>
        public bool IsBreak { get; set; } = false;

        /// <summary>
        /// Исключение (Ошибки)
        /// </summary>
        public Exception Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                _exceptionChangedSubject.OnNext(value);
            }
        }

        /// <summary>
        /// Результат
        /// </summary>
        public Result Result
        {
            get => _result;
            set
            {
                _result = value;
                _resultChangedSubject.OnNext(value);
            }
        }

        /// <summary>
        /// Результат
        /// </summary>
        public State State
        {
            get => _state;
            set
            {
                _state = value;
                _stateChangedSubject.OnNext(value);
            }
        }

        #endregion

        #region Property [Observable]

        public IObservable<bool> IsCheckObservable => _checkChangedSubject.AsObservable();
        public IObservable<bool> IsBreakObservable => _breakChangedSubject.AsObservable();
        public IObservable<Exception> ExceptionObservable => _exceptionChangedSubject.AsObservable();
        public IObservable<Result> ResultObservable => _resultChangedSubject.AsObservable();
        public IObservable<State> StateObservable => _stateChangedSubject.AsObservable();



        #endregion

        

      


        ~ActionDescriptor()
        {
            Count--;
            System.Diagnostics.Debug.WriteLine("ActionDescriptor Count :" + Count);

            if (true)
            {

            }
        }














        public void Dispose()
        {

        }
    }
}
