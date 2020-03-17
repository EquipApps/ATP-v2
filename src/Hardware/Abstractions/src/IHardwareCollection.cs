using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Коллекция у стройств.
    /// (Singleton)
    /// </summary>
    public interface IHardwareCollection : IEnumerable<IHardware>, IEnumerable
    {
        /// <summary>
        /// Возволяет подписаться на изменения коллекции
        /// </summary>        
        IObservable<IChangeSet<IHardware, string>> Connect();

        /// <summary>
        /// Возвращает устройство.
        /// Если устройства нет, возвращает NULL
        /// </summary>       
        IHardware this[string hardwareKey] { get; }

        /// <summary>
        /// Добавляет или обновляет устройство.
        /// </summary>
        void AddOrUpdate(IHardware hardware);

        /// <summary>
        /// Очищает все устройства
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>       
        bool ContainsHardwareWithKey(string hardwareKey);

        /// <summary>
        /// Удаляет устройство
        /// </summary>      
        void Remove(string hardwareKey);
    }
}
