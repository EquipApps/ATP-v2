﻿using EquipApps.Mvc;
using EquipApps.Mvc.Abstractions;
using ReactiveUI;
using System;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionHost : ReactiveObject, IDisposable
    {
        private ActionDescriptor _model;
        private IDisposable _cleanUp;

        ~ActionHost()
        {
            if (true)
            {

            }
        }


        public ActionHost(ActionDescriptor model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));


            //-- Подписываемя на обновление состояня модели

            //var excRefresher = _model.ExceptionObservable
            //    .Subscribe(_ => Exception = _model.Exception);

            //var resRefresher = _model.ResultObservable
            //    .Subscribe(_ => Result = _model.Result);

            //var staRefresher = _model.StateObservable
            //    .Subscribe(_ => State = _model.State);

            ////-- 
            //_cleanUp = Disposable.Create(() =>
            //{
            //    excRefresher.Dispose();
            //    resRefresher.Dispose();
            //    staRefresher.Dispose();
            //});
        }


        public Number Number => _model.Number;
        public string TestCase => _model.TestCase.Title;
        public string TestStep => _model.TestStep.Title;


        #region Property [Reactive]        

        //[Reactive]
        public Exception Exception => _model.Exception;

        //[Reactive]
        public Result Result => _model.Result;

        //[Reactive]
        public State State => _model.State;


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
            //_cleanUp.Dispose();
        }
    }
}