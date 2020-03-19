using System;
using System.Collections;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Реализация <see cref="IRuntimeStateEnumerator"/>.
    /// НЕ потоко безопасна!
    /// </summary>
    public class RuntimeStateEnumerator : IRuntimeStateEnumerator
    {
        private const int indexDefault = -1;
        private       int indexCurrent = -1;

        public int? jumpNext  = null;

        private bool disposedValue = false;

        private RuntimeStateCollection collection;
        

        public RuntimeStateEnumerator(RuntimeStateCollection runtimeStateCollection)
        {
            collection = runtimeStateCollection ?? throw new ArgumentNullException(nameof(runtimeStateCollection));
        }

        public IRuntimeState Current { get; private set; } = null;


        public bool JumpTo(RuntimeStateType stateType)
        {
            IsDisposed();

            //-- Устанавливаем индекс перехода по умолчанию
            var jumpIndex = indexDefault;

            //-- Поиск
            switch (stateType)
            {
                case RuntimeStateType.START:
                    jumpIndex = collection.Index_start;
                    break;

                case RuntimeStateType.INVOKE:
                    jumpIndex = collection.Index_invoke;
                    break;

                case RuntimeStateType.MOVE:
                    jumpIndex = collection.Index_move;
                    break;

                case RuntimeStateType.END:
                    jumpIndex = collection.Index_end;
                    break;

                default:
                    break;

            }

            //-- Если индекс не изменился, то переход не возможен
            if (jumpIndex == indexDefault)
            {
                jumpNext = null;
                return false;
            }
            else
            {
                jumpNext = jumpIndex;
                return true;
            }
        }

        public bool MoveNext()
        {
            IsDisposed();

            //-- Определяе следующий индекс
            int nextIndex;
            if (jumpNext.HasValue)
            {
                nextIndex = jumpNext.Value;
                jumpNext = null;
            }
            else
            {
                nextIndex = indexCurrent + 1;
            }

            //-- Проверка на диапозон
            if (nextIndex > indexDefault && nextIndex < collection.Count)
            {
                indexCurrent = nextIndex;
                Current = collection[nextIndex];
                return true;
            }
            else
            {
                Current = null;
                return false;
            }
        }

        public void Reset()
        {
            IsDisposed();

            //-- Обнуляем счетчик
            indexCurrent = indexDefault;

            //-- Обнуляем
            Current = null;
        }






        public void Dispose()
        {
            disposedValue = true;

            collection = null;
            Current    = null;
        }




        private void IsDisposed()
        {
            if (disposedValue)
                throw new InvalidOperationException("Enumerator disposed of");
        }

        object IEnumerator.Current => Current;
    }
}
