using DynamicData;
using System;
using System.Collections.Generic;

namespace EquipApps.Hardware
{
    //TODO: Написать юнит тест. когда ключ при регистрации ниже по иерархии чем экземпляр

    public class HardwareBehaviorCollection : IHardwareBehaviorCollection, IDisposable
    {
        private IHardware _hardware;
        private SourceCache<IHardwareBehavior, Type> _source;
        private Dictionary<Type, IHardwareBehavior> _source2;

        public HardwareBehaviorCollection(IHardware hardware)
        {
            _hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
            _source = new SourceCache<IHardwareBehavior, Type>(x =>
      {
          return x.GetType();
      });

            _source2 = new Dictionary<Type, IHardwareBehavior>();
        }

        /// <summary>
        /// Возволяет подписаться на изменения коллекции
        /// </summary>   
        public IObservable<IChangeSet<IHardwareBehavior, Type>> Connect() => _source.Connect();

        /// <summary>
        /// Возвращает количиство <see cref="IHardwareBehavior"/>
        /// </summary>
        public int Count => _source2.Count;

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

            _source2.Add(typeof(TBehavior), behavior);
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
            if (_source2.TryGetValue(typeof(TBehavior), out IHardwareBehavior behavior))
                return behavior as TBehavior;
            else
                return null;
        }

        /// <summary>
        /// Удаляет поведение из колекции
        /// </summary>        
        public void Remove<TBehavior>()
            where TBehavior : class, IHardwareBehavior
        {
            _source2.Remove(typeof(TBehavior));
        }

        /// <summary>
        /// Очищает все поведение
        /// </summary>
        public void Clear()
        {
            _source2.Clear();
        }


        /// <summary>
        /// Проверяет, есть ли в коллекции поведение
        /// </summary>       
        public bool ContainsBehaviorWithKey<TBehavior>()
            where TBehavior : class, IHardwareBehavior
        {
            return _source2.ContainsKey(typeof(TBehavior));
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
