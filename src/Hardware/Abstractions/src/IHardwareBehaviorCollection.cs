using DynamicData;
using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Коллекция поведений.
    /// </summary>
    public interface IHardwareBehaviorCollection
    {

        /// <summary>
        /// Позволяет подписаться на изменения коллекции
        /// </summary>    
        IObservable<IChangeSet<IHardwareBehavior, Type>> Connect();

        /// <summary>
        /// Возвращает поведение типа
        /// Если поведения нет, возвращает NULL
        /// </summary>       
        TBehavior Get<TBehavior>()
            where TBehavior : class, IHardwareBehavior;

        /// <summary>
        /// Задает поведение типа
        /// </summary>      
        void AddOrUpdate<TBehavior>(TBehavior instance)
            where TBehavior : class, IHardwareBehavior;

        /// <summary>
        /// 
        /// </summary>       
        bool ContainsBehaviorWithKey<TBehavior>()
            where TBehavior : class, IHardwareBehavior;


    }
}
