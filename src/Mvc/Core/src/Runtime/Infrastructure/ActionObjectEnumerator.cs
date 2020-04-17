using EquipApps.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Реализация <see cref="IActionObjectEnumerator"/>.
    /// НЕ потоко безопасна!
    /// </summary>
    public class ActionObjectEnumerator : IActionObjectEnumerator, IDisposable
    {
        private const int indexDefault = -1;
        private int indexCurrent = -1;

        private int? jumpNext = null;

        private bool disposedValue = false;

        private IReadOnlyList<ActionObject> _actions;


        public ActionObjectEnumerator(IReadOnlyList<ActionObject> actions)
        {
            _actions = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public ActionObject Current { get; private set; } = null;

        public bool JumpTo(ActionObject actionObject)
        {
            IsDisposed();

            //-- Устанавливаем индекс перехода по умолчанию
            int jumpIndex = indexDefault;

            //-- Поиск
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i] == actionObject)
                {
                    jumpIndex = i;
                    break;
                }
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
            if (nextIndex > indexDefault && nextIndex < _actions.Count)
            {
                indexCurrent = nextIndex;
                Current = _actions[nextIndex];
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

            _actions = null;
            Current = null;
        }

        private void IsDisposed()
        {
            if (disposedValue)
                throw new InvalidOperationException("Enumerator disposed of");
        }

        object IEnumerator.Current => Current;
    }
}
