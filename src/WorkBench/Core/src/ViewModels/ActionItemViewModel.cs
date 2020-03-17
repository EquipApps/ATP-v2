using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Objects;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionItemViewModel : ViewModelBase, IDisposable
    {
        private ActionDescriptor _model;
        private IDisposable _cleanUp;

        ~ActionItemViewModel()
        {
            if (true)
            {

            }
        }


        public ActionItemViewModel(ActionDescriptor model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));


            //-- Подписываемя на обновление состояня модели

            var excRefresher = _model.ExceptionObservable
                .Subscribe(_ => Exception = _model.Exception);

            var resRefresher = _model.ResultObservable
                .Subscribe(_ => Result = _model.Result);

            var staRefresher = _model.StateObservable
                .Subscribe(_ => State = _model.State);

            //-- 
            _cleanUp = Disposable.Create(() =>
            {
                excRefresher.Dispose();
                resRefresher.Dispose();
                staRefresher.Dispose();
            });
        }



        public string Area       => _model.Area;
        public TestNumber Number => _model.Number;

        //public string TestSuit => _model.TestSuit.Title;
        public string TestCase => _model.TestCase.Title;
        public string TestStep => _model.TestStep.Title;


        #region Property [Reactive]        

        [Reactive]
        public Exception Exception { get; private set; }

        [Reactive]
        public Result Result { get; private set; }

        [Reactive]
        public State State { get; private set; }


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




        #endregion


        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}