using EquipApps.Mvc.Abstractions;
using System;
using System.Reactive.Subjects;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Дескриптер действия
    /// </summary>
    public abstract partial class ActionDescriptor : IDisposable
    {
        private readonly ISubject<State> _stateChangedSubject = new ReplaySubject<State>();

        public IObservable<State> StateObservable => _stateChangedSubject;

        public void SetState(State state)
        {
            _stateChangedSubject.OnNext(state);
        }

        /// <summary>
        /// Возвращает <see cref="Abstractions.ActionDescriptorResult"/>
        /// </summary>
        public ActionDescriptorResult Result { get; private set; }

        

        public void SetResult(
            ActionDescriptorResultType resultType,
            Exception exception = null)
        {
            Result = new ActionDescriptorResult(resultType, exception);
        }


        public ActionDescriptor(ActionDescriptorObject testCase, ActionDescriptorObject testStep)
            :this()
        {
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
            TestStep = testStep ?? throw new ArgumentNullException(nameof(testStep));

            Result = new ActionDescriptorResult(ActionDescriptorResultType.NotRun);
        }

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

       

        /// <summary>
        /// Флаг. Указывает будет ли элемент участвовать в проверке.
        /// </summary>
        public bool IsCheck { get; set; } = true;

        /// <summary>
        /// Флаг. Точки остановки
        /// </summary>
        public bool IsBreak { get; set; } = false;







        public void Dispose()
        {
            //TODO: Должна удаляться при очистке кеша!
            _stateChangedSubject.OnCompleted();
        }
    }
}
