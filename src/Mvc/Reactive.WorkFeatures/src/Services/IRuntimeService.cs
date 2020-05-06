using System;

namespace EquipApps.Mvc.Reactive.WorkFeatures.Services
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Сервис управления процессом проверки.
    /// </summary>
    public interface IRuntimeService
    {
        /// <summary>
        /// Разрешить / запретить цикл всех действий
        /// </summary>
        void EnabledRepeat(bool isRepeatEnabled);

        /// <summary>
        /// Разрешить / запретить цикл одного действия
        /// </summary>
        void EnabledRepeatOnce(bool isRepeatOnceEnabled);

        // <summary>
        /// Разрешить / запретить пошаговый режим
        /// </summary>
        void EnabledPause(bool isPauseEnabled);

        /// <summary>
        /// Позволяет подписаться на события счетчика циклов  всех действий
        /// </summary>
        /// 
        /// <remarks>
        /// -1 -> Вкл. Бесконечный цикл.
        ///  0 -> Выкл.
        ///  1 -> Вкл. Счетчик.
        /// </remarks>
        /// 
        IObservable<int> ObservableCountRepeat { get; }

        /// <summary>
        /// Позволяет подписаться на события счетчика циклов одного действия
        /// </summary>
        /// 
        /// <remarks>
        /// -1 -> Вкл. Бесконечный цикл.
        ///  0 -> Выкл.
        ///  1 -> Вкл. Счетчик.
        /// </remarks>
        /// 
        IObservable<int> ObservableCountRepeatOnce { get; }

        /// <summary>
        /// Позволяет подписаться на события изменения состояния паузы
        /// </summary>
        /// 
        /// <remarks>
        /// Может возникать как в пошаговом режиме, так и во время точек остоновок
        /// </remarks>
        /// 
        IObservable<bool> ObservablePause { get; }

        /// <summary>
        /// Навигация. Шаг вперед.
        /// </summary>
        void Next();

        /// <summary>
        /// Навигация. Повтор.
        /// </summary>
        void Replay();

        /// <summary>
        /// Навигация. Шаг назад.
        /// </summary>
        void Previous();
    }
}
