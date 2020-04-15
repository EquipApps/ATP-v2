using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionHost : ReactiveObject, IDisposable
    {
        private ActionDescriptor _model;
        private IDisposable _cleanUp;

        public ActionHost(ActionDescriptor model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            State = State.Empy;

            //-- Подписываемя на обновление состояня модели

            var staRefresher = _model.StateObservable.ObserveOn(RxApp.MainThreadScheduler)
                                                     .Subscribe(NotifyProperty);

            //-- 
            _cleanUp = Disposable.Create(() =>
            {
                staRefresher.Dispose();
            });
        }

        public Number Number    => _model.Number;
        public string TestCase  => _model.TestCase.Title;
        public string TestStep  => _model.TestStep.Title;
        public Exception Exception => _model.Result.Exception;
        public ActionDescriptorResultType Result => _model.Result.Type;

        [Reactive] public State State { get; private set; }

        public bool IsBreak
        {
            get => _model.IsBreak;
            set
            {
                _model.IsBreak = value;
                this.RaisePropertyChanged(nameof(IsBreak));
            }
        }
        public bool IsCheck
        {
            get => _model.IsCheck;
            set
            {
                _model.IsCheck = value;
                this.RaisePropertyChanged(nameof(IsCheck));
            }
        }
        public void Dispose()
        {
            _cleanUp.Dispose();
        }


        private void NotifyProperty(State state)
        {
            State = state;
        }
    }
}