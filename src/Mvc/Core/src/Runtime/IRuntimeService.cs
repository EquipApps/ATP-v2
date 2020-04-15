using System;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// DependencyInjection (Singleton).
    /// Сервис управления процессом проверки.
    /// </summary>
    public interface IRuntimeService
    {
        // <summary>
        /// Разрешить / запретить цикл всех действий
        /// </summary>
        bool IsEnabledRepeat { get; set; }

        // <summary>
        /// Разрешить / запретить цикл одного действия
        /// </summary>
        bool IsEnabledRepeatOnce { get; set; }

        // <summary>
        /// Разрешить / запретить пошаговый режим
        /// </summary>
        bool IsEnabledPause { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        int MillisecondsTimeout { get; set; }

        /// <summary>
        /// Позволчят подписаться на события изменения состояния паузы
        /// </summary>
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
