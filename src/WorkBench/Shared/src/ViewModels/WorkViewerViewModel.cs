using EquipApps.Testing;
using EquipApps.Mvc.Runtime;
using EquipApps.WorkBench.Services;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using EquipApps.Mvc.Services;

namespace EquipApps.WorkBench.ViewModels
{
    //TODO: Рефракторинг
    public partial class WorkViewerViewModel
    {
        private IDisposable _cleanUp;
        private ITestFactory testFactory;
      
        private IActionService testService;
        private ILogEntryService logsService;


        private IRuntimeService runtimeService;


        public static volatile int Countter = 0;

        private void ctor(
            IRuntimeService runtimeService,
            ITestFactory testFactory,
            IActionService actionDescripterService,
            ILogEntryService logEntryService)
        {
            this.testFactory = testFactory        ?? throw new ArgumentNullException(nameof(testFactory));
            testService = actionDescripterService ?? throw new ArgumentNullException(nameof(actionDescripterService));
            logsService = logEntryService         ?? throw new ArgumentNullException(nameof(logEntryService));
            this.runtimeService = runtimeService  ?? throw new ArgumentNullException(nameof(runtimeService));




            //-- ПОДПИСЫВАЕМСЯ
            var pausedRefresher = runtimeService.ObservablePause
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(isPaued => IsPausing = isPaued);


            #region build TestBuild ReactiveCommand

            var canTestBuild = this
               .WhenAnyValue(
                    x => x.IsBuilding,
                    x => x.IsTesting,
                    (building, testing) => !building &&
                                           !testing);
            TestCreate = ReactiveCommand.CreateFromTask(TestCreateAsync, canTestBuild);


            var canTestClean = this
               .WhenAnyValue(
                    x => x.IsBuilding,
                    x => x.HesTest,
                    x => x.IsTesting,
                    (building, build, testing) => !building &&
                                                   build &&
                                                  !testing);
            TestClean = ReactiveCommand.CreateFromTask(TestCleanAsync, canTestClean);

            #endregion

            #region build Test ReactiveCommand

            var canTestStart = this
               .WhenAnyValue(
                    x => x.IsBuilding,
                    x => x.HesTest,
                    x => x.IsTesting,
                    (building, build, testing) => !building &&
                                                   build &&
                                                  !testing);
            TestStart = ReactiveCommand.CreateFromTask(TestStartAsync, canTestStart);


            var canTestStop = this
                .WhenAnyValue(
                    x => x.HesTest,
                    x => x.IsTesting,
                    (building, testing) => testing);
            TestStop = ReactiveCommand.Create(TestCancel, canTestStop);



            TestPause       = ReactiveCommand.Create(OnTestPauseTask);
            TestRepeat      = ReactiveCommand.Create(OnTestRepeatTask);
            TestRepeatOnce  = ReactiveCommand.Create(OnTestRepeatOnceTask);



            var canTestNext = this
                .WhenAnyValue(
                    x => x.IsTesting,
                    x => x.IsPausing,
                    (testing, pause) => testing &&
                                        pause);
            TestPrevious = ReactiveCommand.Create(OnTestPreviousTask, canTestNext);
            TestReplay = ReactiveCommand.Create(OnTestReplayTask, canTestNext);
            TestNext = ReactiveCommand.Create(OnTestNextTask, canTestNext);


            #endregion 
        }


       

       


        

        private void OnTestRepeatOnceTask()
        {
            runtimeService.IsEnabledRepeatOnce = IsRepeatOnceEnabled;
        }

        private void OnTestRepeatTask()
        {
            runtimeService.IsEnabledRepeat = IsRepeatEnabled;
        }

        private void OnTestPauseTask()
        {
            runtimeService.IsEnabledPause = IsPauseEnabled;
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
