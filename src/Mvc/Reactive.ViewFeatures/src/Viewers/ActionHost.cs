using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Viewers
{
    public class ActionHost : ReactiveObject, IDisposable
    {
        private ActionObject _model;
        private IDisposable _cleanUp;

        public ActionHost(ActionObject model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            State = ActionObjectState.Empy;

            //-- Подписываемя на обновление состояня модели

            var staRefresher = _model.StateObservable.ObserveOn(RxApp.MainThreadScheduler)
                                                     .Subscribe(NotifyProperty);

            //-- 
            _cleanUp = Disposable.Create(() =>
            {
                staRefresher.Dispose();
            });
        }

        public Number Number => _model.ActionDescriptor.Number;
        public string TestCase => _model.ActionDescriptor.TestCase.Title;
        public string TestStep => _model.ActionDescriptor.TestStep.Title;
        public Exception Exception => _model.Result.Exception;
        public ActionObjectResultType Result => _model.Result.Type;

        [Reactive] public ActionObjectState State { get; private set; }

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


        private void NotifyProperty(ActionObjectState state)
        {
            State = state;
        }
    }
}