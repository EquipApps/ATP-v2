using System;

namespace EquipApps.Hardware
{
    /// <summary>
    /// Инфроструктура "виртуальное" устройство
    /// </summary>
    public class Hardware : IHardware, IDisposable
    {
        private HardwareBehaviorCollection behaviorCollection;

        public Hardware(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Name = key;
            Description = string.Empty;
            behaviorCollection = new HardwareBehaviorCollection(this);
        }

        /// <summary>
        /// Возвращает ключ устройства. Уникальное!
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// Возвращает имя устройства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает имя устройства. Уникальное имя!
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Возвращает <see cref="IHardwareBehaviorCollection"/> для данного устройства
        /// </summary>
        public IHardwareBehaviorCollection Behaviors => behaviorCollection;


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    behaviorCollection.Dispose();
                }

                behaviorCollection = null;
                disposedValue = true;
            }
        }

        ~Hardware()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
