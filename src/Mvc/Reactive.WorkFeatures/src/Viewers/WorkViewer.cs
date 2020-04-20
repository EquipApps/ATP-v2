﻿using EquipApps.Testing;
using EquipApps.Mvc.Runtime;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewer : ReactiveObject
    {
        private IDisposable _cleanUp;
        private ITestFactory testFactory;
        private IRuntimeService runtimeService;


        public static volatile int Countter = 0;

        public WorkViewer(
            IRuntimeService runtimeService,
            ITestFactory testFactory)
        {
            this.testFactory = testFactory        ?? throw new ArgumentNullException(nameof(testFactory));
            this.runtimeService = runtimeService  ?? throw new ArgumentNullException(nameof(runtimeService));


            //-- ПОДПИСЫВАЕМСЯ
            var pausedRefresher = runtimeService.ObservablePause.ObserveOn(RxApp.MainThreadScheduler)
                                                                .Subscribe(isPaued => IsPausing = isPaued);

            var canTestBuild = this.WhenAnyValue(x => x.IsBuilding,
                                                 x => x.IsTesting,
                                                 (building, testing) => !building && !testing);

            var canTestClean = this.WhenAnyValue(x => x.IsBuilding, 
                                                 x => x.HesTest,
                                                 x => x.IsTesting,
                                                 (building, build, testing) => !building && build && !testing);

            var canTestStart = this.WhenAnyValue(x => x.IsBuilding,
                                                 x => x.HesTest,
                                                 x => x.IsTesting,
                                                 (building, build, testing) => !building && build && !testing);

            var canTestStop = this.WhenAnyValue(x => x.HesTest,
                                                x => x.IsTesting,
                                                (building, testing) => testing);

            var canTestNext = this.WhenAnyValue(x => x.IsTesting,
                                                x => x.IsPausing,
                                                (testing, pause) => testing && pause);

            //--
            TestCreate  = ReactiveCommand.CreateFromTask(TestCreateAsync, canTestBuild);            
            TestClean   = ReactiveCommand.CreateFromTask(TestCleanAsync,  canTestClean);         
            TestStart   = ReactiveCommand.CreateFromTask(TestStartAsync,  canTestStart);

            //--
            TestStop    = ReactiveCommand.Create(TestCancel, canTestStop);

            //--
            TestPause       = ReactiveCommand.Create(OnTestPauseTask);
            TestRepeat      = ReactiveCommand.Create(OnTestRepeatTask);
            TestRepeatOnce  = ReactiveCommand.Create(OnTestRepeatOnceTask);

            //--
            TestPrevious    = ReactiveCommand.Create(OnTestPreviousTask, canTestNext);
            TestReplay      = ReactiveCommand.Create(OnTestReplayTask, canTestNext);
            TestNext        = ReactiveCommand.Create(OnTestNextTask, canTestNext);
        }


        /// <summary>
        /// 
        /// </summary>
        [Reactive] public bool IsPauseEnabled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public bool IsRepeatEnabled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Reactive] public bool IsRepeatOnceEnabled { get; set; } = false;



        /// <summary>
        /// Флаг. Тест создан
        /// </summary>
        [Reactive] public bool HesTest { get; private set; } = false;

        /// <summary>
        /// Флаг. Идет создание/удаление тестовой проверки
        /// </summary>
        [Reactive] public bool IsBuilding { get; private set; } = false;

        /// <summary>
        /// Флаг. Идет проверка
        /// </summary>
        [Reactive] public bool IsTesting { get; private set; } = false;

        /// <summary>
        /// Флаг. Проверка находится в состоянии паузы
        /// </summary>
        [Reactive] public bool IsPausing { get; private set; } = false;


        public ReactiveCommand<Unit, Unit> TestCreate 
        { 
            get; private set; 
        }
        public ReactiveCommand<Unit, Unit> TestClean
        {
            get; private set;
        }


        public ReactiveCommand<Unit, Unit> TestStart
        {
            get; private set;
        }
        public ReactiveCommand<Unit, Unit> TestStop
        {
            get; private set;
        }


        public ReactiveCommand<Unit, Unit> TestPause
        {
            get; private set;
        }
        public ReactiveCommand<Unit, Unit> TestRepeat
        {
            get; private set;
        }
        public ReactiveCommand<Unit, Unit> TestRepeatOnce
        {
            get; private set;
        }


        public ReactiveCommand<Unit, Unit> TestPrevious
        {
            get; private set;
        }
        public ReactiveCommand<Unit, Unit> TestReplay
        {
            get; private set;
        }
        public ReactiveCommand<Unit, Unit> TestNext
        {
            get; private set;
        }


        private void OnTestRepeatOnceTask()
        {
            runtimeService.EnabledRepeatOnce(IsRepeatOnceEnabled);
        }
        private void OnTestRepeatTask()
        {
            runtimeService.EnabledRepeat(IsRepeatEnabled);
        }
        private void OnTestPauseTask()
        {
            runtimeService.EnabledPause(IsPauseEnabled);
        }


        private void OnTestNextTask()
        {
            runtimeService.Next();
        }
        private void OnTestReplayTask()
        {
            runtimeService.Replay();
        }
        private void OnTestPreviousTask()
        {
            runtimeService.Previous();
        }
    }
}