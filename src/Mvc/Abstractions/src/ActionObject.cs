using System;
using System.Reactive.Subjects;

namespace EquipApps.Mvc
{
    public class ActionObject : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ISubject<ActionObjectState> _stateChangedSubject = new ReplaySubject<ActionObjectState>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        public ActionObject(ActionDescriptor actionDescriptor)
        {
            ActionDescriptor = actionDescriptor ?? throw new ArgumentNullException(nameof(actionDescriptor));
            Result = new ActionObjectResult(ActionObjectResultType.NotRun);
        }

        public string Id => ActionDescriptor.Id;


        /// <summary>
        /// 
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; }

        /// <summary>
        /// Возвращает <see cref="ActionObjectResult"/>
        /// </summary>
        public ActionObjectResult Result 
        { 
            get; private set; 
        }

        /// <summary>
        /// Возвращает <see cref="ActionObjectResult"/>
        /// </summary>
        public ActionObjectState State
        {
            get; private set;
        }

        /// <summary>
        /// Флаг. Указывает будет ли элемент участвовать в проверке.
        /// </summary>
        public bool IsCheck { get; set; } = true;

        /// <summary>
        /// Флаг. Точки остановки
        /// </summary>
        public bool IsBreak { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public IObservable<ActionObjectState> StateObservable
        {
            get => _stateChangedSubject;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(ActionObjectState state)
        {
            State = state;
            _stateChangedSubject.OnNext(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetResult(
            ActionObjectResultType resultType,
            Exception exception = null)
        {
            Result = new ActionObjectResult(resultType, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _stateChangedSubject.OnCompleted();
        }
    }
}
