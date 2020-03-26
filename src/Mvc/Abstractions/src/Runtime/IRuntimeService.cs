using System;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Сервис управления процессом проверки.
    /// Singleton
    /// </summary>
    public interface IRuntimeService: IDisposable
    {
        // <summary>
        /// Разрешить / запретить цикл всех действий
        /// </summary>
        bool IsEnabledRepeat { get; set; }

        // <summary>
        /// Разрешить / запретить цикл одного действия
        /// </summary>
        bool IsEnabledRepeatOnce { get; set; }


        
        int MillisecondsTimeout { get; set; }



        /// <summary>
        /// Разрешить / запретить пошаговый режим
        /// </summary>
        bool IsEnabledPause { get; set; }

        /// <summary>
        /// Позволчят подписаться на события изменения состояния паузы
        /// </summary>
        IObservable<bool> ObservablePause { get; }
















        /// <summary>
        /// 
        /// </summary>
        void Next();

        /// <summary>
        /// 
        /// </summary>
        void Replay();

        /// <summary>
        /// 
        /// </summary>
        void Previous();
    }
}
