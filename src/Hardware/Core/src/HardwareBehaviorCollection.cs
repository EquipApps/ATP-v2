using DynamicData;
using System;

namespace EquipApps.Hardware
{

    public class HardwareBehaviorCollection : IHardwareBehaviorCollection, IDisposable
    {
        private IHardware _hardware;
        private SourceCache<IHardwareBehavior, Type> _source;

        public HardwareBehaviorCollection(IHardware hardware)
        {
            _hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
            _source = new SourceCache<IHardwareBehavior, Type>(x =>
      {
          return x.GetType();
      });
        }

        /// <summary>
        /// Возволяет подписаться на изменения коллекции
        /// </summary>   
        public IObservable<IChangeSet<IHardwareBehavior, Type>> Connect() => _source.Connect();

        /// <summary>
        /// Возвращает количиство <see cref="IHardwareBehavior"/>
        /// </summary>
        public int Count => _source.Count;

        /// <summary>
        /// Добавляет или обновляет поведение.
        /// </summary>        
        public void AddOrUpdate<TBehavior>(TBehavior behavior)
            where TBehavior : class, IHardwareBehavior
        {
            if (behavior == null)
            {
                throw new ArgumentNullException(nameof(behavior));
            }

            _source.AddOrUpdate(behavior);

            behavior.Hardware = _hardware;
            behavior.Attach();
        }

        /// <summary>
        /// Извлекает поведение из колекции. если его нет возврашает bull.
        /// </summary>        
        public TBehavior Get<TBehavior>()
            where TBehavior : class, IHardwareBehavior
        {
            var result = _source.Lookup(typeof(TBehavior));
            if (result.HasValue)
                return result.Value as TBehavior;
            else
                return null;
        }

        /// <summary>
        /// Удаляет поведение из колекции
        /// </summary>        
        public void Remove<TBehavior>()
            where TBehavior : class, IHardwareBehavior
        {
            _source.RemoveKey(typeof(TBehavior));
        }

        /// <summary>
        /// Очищает все поведение
        /// </summary>
        public void Clear()
        {
            _source.Clear();
        }


        /// <summary>
        /// Проверяет, есть ли в коллекции поведение
        /// </summary>       
        public bool ContainsBehaviorWithKey<TBehavior>()
            where TBehavior : class, IHardwareBehavior
        {
            return _source.Lookup(typeof(TBehavior)).HasValue;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in _source.Items)
                    {
                        try
                        {
                            (item as IDisposable)?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            //TODO: Добавить логер
                        }
                    }

                    _source.Dispose();
                }

                _hardware = null;
                _source = null;

                disposedValue = true;
            }
        }


        ~HardwareBehaviorCollection()
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
