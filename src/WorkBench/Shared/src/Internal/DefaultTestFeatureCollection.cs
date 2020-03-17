using EquipApps.Testing.Features;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EquipApps.WorkBench.Internal
{
    /// <summary>
    /// Коллекция расширений
    /// </summary>
    public class DefaultTestFeatureCollection : IFeatureCollection, IDisposable
    {
        private IDictionary<Type, object> _features = new Dictionary<Type, object>();

        //--
        public object this[Type key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                _features.TryGetValue(key, out object result);

                return result;

            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    _features.Remove(key);
                    return;
                }

                _features[key] = value;

            }
        }

        //--
        public TFeature Get<TFeature>()
        {
            return (TFeature)this[typeof(TFeature)];
        }

        //--
        public void Set<TFeature>(TFeature instance)
        {
            this[typeof(TFeature)] = instance;
        }

        //--
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            foreach (var pair in _features)
            {
                yield return pair;
            }
        }

        //--
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //dispose managed state (managed objects).
                    foreach (var item in _features)
                    {
                        try
                        {
                            (item.Value as IDisposable)?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            //TODO: Add logger;
                        }


                    }
                }

                //set large fields to null.
                _features = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
