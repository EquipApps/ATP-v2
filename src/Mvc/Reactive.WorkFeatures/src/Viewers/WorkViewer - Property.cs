using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Windows.Input;

namespace EquipApps.WorkBench.ViewModels
{
    //TODO: Рефракторинг
    public partial class WorkViewer
    {
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


        #region Command

        public ReactiveCommand<Unit,Unit>  TestCreate { get; private set; }

        public ReactiveCommand<Unit, Unit> TestClean { get; private set; }










        public ICommand TestStart { get; private set; }
        public ICommand TestStop { get; private set; }


        public ICommand TestPause { get; private set; }
        public ICommand TestRepeat { get; private set; }
        public ICommand TestRepeatOnce { get; private set; }


        public ICommand TestPrevious { get; private set; }
        public ICommand TestReplay { get; private set; }
        public ICommand TestNext { get; private set; }

        #endregion
    }
}
