using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Runtime.Internal
{
    public class RuntimeActionDescriptorEnumerator : IRuntimeEnumerator, IDisposable
    {
        private IReadOnlyList<ActionDescriptor> _descriptors;
        private ActionDescriptor _descriptor;

        private const int _indexDefault = -1;
        private volatile int _index = -1;
        private int? _indexNext = null;

        public RuntimeActionDescriptorEnumerator(IReadOnlyList<ActionDescriptor> descriptors)
        {
            _descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
            _descriptor = null;
        }

        public bool JumpTo(ActionDescriptor actionDescriptor)
        {
            if (disposedValue)
                throw new InvalidOperationException("Enumerator disposed of");

            var index = -1;
            for (int i = 0; i < _descriptors.Count; i++)
            {
                if (_descriptors[i] == actionDescriptor)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return false;
            else
            {
                _indexNext = index;
                return true;
            }
        }

        #region Enumerator pattern

        public ActionDescriptor Current => _descriptor;

        object IEnumerator.Current => _descriptor;

        public bool MoveNext()
        {
            if (disposedValue)
                throw new InvalidOperationException("Enumerator disposed of");

            if (_indexNext.HasValue)
            {
                _index = _indexNext.Value;
                _indexNext = null;
            }
            else
            {
                //-- Увеличиваем счетчик
                _index++;
            }

            return TryUpdate();
        }

        public void Reset()
        {
            if (disposedValue)
                throw new InvalidOperationException("Enumerator disposed of");

            //-- Обнуляем счетчик
            _index = _indexDefault;

            //-- Обнуляем
            _descriptor = null;
        }


        #endregion

        private bool TryUpdate()
        {
            if ((_index > _indexDefault) && (_index < _descriptors.Count))
            {
                _descriptor = _descriptors[_index];
                return true;
            }
            else
            {
                _descriptor = null;
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _descriptors = null;
                    _descriptor = null;

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RuntimeEnumerator() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
