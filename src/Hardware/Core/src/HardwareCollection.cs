using DynamicData;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    public class HardwareCollection : IHardwareCollection
    {
        private ILogger<HardwareCollection> _logger;
        private SourceCache<IHardware, string> _source = new SourceCache<IHardware, string>(x => x.Key);

        public HardwareCollection(ILogger<HardwareCollection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Возволяет подписаться на изменения коллекции
        /// </summary>     
        public IObservable<IChangeSet<IHardware, string>> Connect() => _source.Connect();

        /// <summary>
        /// Возвращает устройство.
        /// Если устройства нет, возвращает NULL
        /// </summary>     
        public IHardware this[string hardwareKey]
        {
            get
            {
                if (hardwareKey == null)
                {
                    throw new ArgumentNullException(nameof(hardwareKey));
                }

                var result = _source.Lookup(hardwareKey);
                if (result.HasValue)
                    return result.Value;
                else
                    return null;
            }
        }

        //---
        public int Count
        {
            get => _source.Count;
        }

        /// <summary>
        /// Добавляет или обновляет устройство.
        /// </summary>
        public void AddOrUpdate(IHardware hardware)
        {
            if (hardware == null)
            {
                throw new ArgumentNullException(nameof(hardware));
            }

            if (hardware.Key == null)
            {
                throw new NullReferenceException(nameof(hardware.Key));
            }

            _source.AddOrUpdate(hardware);
        }

        /// <summary>
        /// Удаляет устройство
        /// </summary>   
        public void Remove(string hardwareKey)
        {
            if (hardwareKey == null)
            {
                throw new ArgumentNullException(nameof(hardwareKey));
            }

            _source.RemoveKey(hardwareKey);
        }

        /// <summary>
        /// Очищает все устройства
        /// </summary>
        public void Clear()
        {
            _source.Edit(ClearAction);
        }

        //---
        public bool ContainsHardwareWithKey(string hardwareKey)
        {
            if (hardwareKey == null)
            {
                throw new ArgumentNullException(nameof(hardwareKey));
            }

            return _source.Lookup(hardwareKey).HasValue;
        }

        //---
        private void ClearAction(ISourceUpdater<IHardware, string> source)
        {
            foreach (var hardware in source.Items)
            {
                try
                {
                    (hardware as IDisposable)?.Dispose();
                }
                catch (Exception ex)
                {
                    //TODO: Добавить логер!
                }
            }

            source.Clear();

        }










        public IEnumerator<IHardware> GetEnumerator()
        {
            return _source.Items.GetEnumerator();
        }
        //---

        //---
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
