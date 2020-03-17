using EquipApps.Mvc.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Testing.Mvc.Internal
{
    public class MvcRuntimeStateEnumerator : IRuntimeStateEnumerator, IEnumerator<IRuntimeState>, IEnumerator, IDisposable
    {
        private const int indexNull = -1;
        private volatile int indexCrnt = -1;
        private IRuntimeStateCollection collection;


        public MvcRuntimeStateEnumerator(IRuntimeStateCollection runtimeStateCollection)
        {
            collection = runtimeStateCollection ?? throw new ArgumentNullException(nameof(runtimeStateCollection));
        }

        public IRuntimeState Current { get; private set; } = null;

        public int? IndexNext { get; private set; } = null;

        object IEnumerator.Current => Current;



        public bool JumpTo(RuntimeStateType stateType)
        {
            if (disposedValue) return false;

            var index = indexNull;

            switch (stateType)
            {
                case RuntimeStateType.START:
                    index = collection.Index_start;
                    break;

                case RuntimeStateType.INVOKE:
                    index = collection.Index_invoke;
                    break;

                case RuntimeStateType.MOVE:
                    index = collection.Index_move;
                    break;

                case RuntimeStateType.END:
                    index = collection.Index_end;
                    break;

                default:
                    break;

            }

            if (index != indexNull)
            {
                IndexNext = index;
                return true;
            }

            return false;
        }

        public bool MoveNext()
        {
            if (disposedValue) return false;

            if (IndexNext.HasValue)
            {
                indexCrnt = IndexNext.Value;
                IndexNext = null;
            }
            else
            {
                //-- Увеличиваем счетчик
                indexCrnt++;
            }

            return TryUpdate();
        }

        public void Reset()
        {
            if (disposedValue) return;

            //-- Обнуляем счетчик
            indexCrnt = indexNull;

            //-- Обнуляем
            Current = null;
        }

        private bool TryUpdate()
        {
            if ((indexCrnt > indexNull) && (indexCrnt < collection.Count))
            {
                Current = collection[indexCrnt];
                return true;
            }
            else
            {
                Current = null;
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
                }

                //  set large fields to null.
                collection = null;
                Current = null;
                IndexNext = null;

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
