using EquipApps.Testing;
using EquipApps.WorkBench.Services;
using Microsoft.Extensions.Options;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Testing.Mvc.Runtime;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    //TODO: Рефракторинг
    public partial class WorkViewerViewModel
    {
        private IDisposable _cleanUp;
        private ITestFactory testFactory;
      
        private IActionService testService;
        private ILogEntryService logsService;

       
        


        public static volatile int Countter = 0;

        private void ctor(
            ITestFactory testFactory,
            IActionService actionDescripterService,
            ILogEntryService logEntryService,
            IOptions<MvcOption> options)
        {
            this.testFactory = testFactory        ?? throw new ArgumentNullException(nameof(testFactory));
            testService = actionDescripterService ?? throw new ArgumentNullException(nameof(actionDescripterService));
            logsService = logEntryService         ?? throw new ArgumentNullException(nameof(logEntryService));

            var mvcOption = options?.Value        ?? throw new ArgumentNullException(nameof(options));

            this.runtimeStatePause = mvcOption.RuntimeStates
                .Where  (x => x.Value is IRuntimeStatePause)
                .Select (x => x.Value as IRuntimeStatePause)
                .FirstOrDefault();

            this.runtimeStateRepeatOnce = mvcOption.RuntimeStates
                .Where  (x => x.Value is IRuntimeStateRepeatOnce)
                .Select (x => x.Value as IRuntimeStateRepeatOnce)
                .FirstOrDefault();

            this.runtimeStateRepeat = mvcOption.RuntimeStates
                .Where  (x => x.Value is IRuntimeStateRepeat)
                .Select (x => x.Value as IRuntimeStateRepeat)
                .FirstOrDefault();

            //-- ПОДПИСЫВАЕМСЯ
            var pausedRefresher = runtimeStatePause.IsPausedObservable
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => IsPausing = runtimeStatePause.IsPaused);


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
            runtimeStateRepeatOnce.IsEnabled = IsRepeatOnceEnabled;
        }

        private void OnTestRepeatTask()
        {
            runtimeStateRepeat.IsEnabled = IsRepeatEnabled;
        }

        private void OnTestPauseTask()
        {
            runtimeStatePause.IsEnabled = IsPauseEnabled;
        }


       

        











        private void OnTestNextTask()
        {
            runtimeStatePause.Next();
        }

        private void OnTestReplayTask()
        {
            runtimeStatePause.Replay();
        }

        private void OnTestPreviousTask()
        {
            runtimeStatePause.Previous();
        }
    }
}
