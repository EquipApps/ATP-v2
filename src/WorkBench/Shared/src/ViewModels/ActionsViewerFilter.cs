using EquipApps.Mvc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    /// <summary>
    /// Филтр для <see cref="ActionDescriptor"/>.
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
        public IObservable<Func<ActionDescriptor, bool>> ObservableFilter { get; }

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

        private Func<ActionDescriptor, bool> UpdateFilter((bool showPassed, bool showFailed, bool showNotRun) arg)
        {
            return (actionDescriptor) =>
            {
                switch (actionDescriptor.Result)
                {
                    case Mvc.Abstractions.Result.NotRun:
                        return arg.showNotRun;
                    case Mvc.Abstractions.Result.Passed:
                        return arg.showPassed;
                    case Mvc.Abstractions.Result.Failed:
                        return arg.showFailed;
                    default:
                        return false;
                }
            };
        }
    }
}
