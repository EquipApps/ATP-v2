using DynamicData;
using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Коллекция поведений.
    /// </summary>
    public interface IHardwareBehaviorCollection
    {
        //TODO: Написать юнит тест. когда ключ при регистрации ниже по иерархии чем экземпляр

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
