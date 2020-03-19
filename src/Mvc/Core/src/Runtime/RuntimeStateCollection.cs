using System.Collections;
using System.Collections.Generic;

namespace EquipApps.Mvc.Runtime
{
    /// <summary>
    /// Коллекция <see cref="IRuntimeState"/>
    /// </summary>
    public class RuntimeStateCollection : IReadOnlyCollection<IRuntimeState>
    {
        private IReadOnlyList<IRuntimeState> _arrayStates;

        internal RuntimeStateCollection(IReadOnlyList<IRuntimeState> arrayStates, int index_start, int index_invoke, int index_move, int index_end)
        {
            _arrayStates     = arrayStates;
            Index_start      = index_start;
            Index_invoke     = index_invoke;
            Index_move       = index_move;
            Index_end        = index_end;
        }

        public IRuntimeState this[int index]
        {
            get => _arrayStates[index];
        }

        public int Index_start { get; }
        public int Index_invoke { get;}
        public int Index_move { get; }
        public int Index_end { get; }
        public int Count => _arrayStates.Count;

        public IRuntimeStateEnumerator GetEnumerator()
        {
            return new RuntimeStateEnumerator(this);
        }

        IEnumerator<IRuntimeState> IEnumerable<IRuntimeState>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
