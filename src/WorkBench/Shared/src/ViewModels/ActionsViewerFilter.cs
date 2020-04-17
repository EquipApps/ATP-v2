using EquipApps.Mvc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    /// <summary>
    /// Филтр для <see cref="ActionObject"/>.
    /// </summary>
    public class ActionsViewerFilter : ReactiveObject
    {
        private const double throttleMilliseconds = 500.0;

        public ActionsViewerFilter()
        {
            //-- Обновляет фильт при изменении одного из свойств, не чаще чем в 500 мс.
            ObservableFilter = this.WhenAnyValue(x => x.ShowPassed,
                                                 x => x.ShowFailed,
                                                 x => x.ShowNotRun)
                                   .Throttle(TimeSpan.FromMilliseconds(throttleMilliseconds))
                                   .Select(UpdateFilter);

            //-- Разрешает очищать фильтры, когда хотябы один отключен.
            var canClear = this.WhenAnyValue(x => x.ShowPassed,
                                             x => x.ShowFailed,
                                             x => x.ShowNotRun,
                                             (showPassed, showFailed, showNotRun) => !showPassed || !showFailed || !showNotRun);  //TODO: Оптимизировать логику

            //-- Создаем команду очистки фильтров
            Clear = ReactiveCommand.Create(OnClear, canClear);
        }


        /// <summary>
        /// Предоставляет подписку на обновление Фильтра
        /// </summary>
        public IObservable<Func<ActionObject, bool>> ObservableFilter { get; }

        /// <summary>
        /// Флаг.
        /// </summary>
        [Reactive] public bool ShowPassed { get; set; } = true;

        /// <summary>
        /// Флаг.
        /// </summary>
        [Reactive] public bool ShowFailed { get; set; } = true;

        /// <summary>
        /// Флаг.
        /// </summary>
        [Reactive] public bool ShowNotRun { get; set; } = true;

        /// <summary>
        /// Команда очистки фильтров
        /// </summary>
        public ReactiveCommand<Unit, Unit> Clear { get; }

        private void OnClear()
        {
            ShowPassed = true;
            ShowFailed = true;
            ShowNotRun = true;
        }

        private Func<ActionObject, bool> UpdateFilter((bool showPassed, bool showFailed, bool showNotRun) arg)
        {
            return (actionObject) =>
            {
                switch (actionObject.Result.Type)
                {
                    case ActionObjectResultType.NotRun:
                        return arg.showNotRun;
                    case ActionObjectResultType.Passed:
                        return arg.showPassed;
                    case ActionObjectResultType.Failed:
                        return arg.showFailed;
                    default:
                        return false;
                }
            };
        }
    }
}
