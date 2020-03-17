using EquipApps.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Testing.Mvc.Internal
{
    public class MvcRuntimeStateCollection : IRuntimeStateCollection
    {
        private IRuntimeState[] states;

        public MvcRuntimeStateCollection(IEnumerable<KeyValuePair<RuntimeStateId, IRuntimeState>> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            var arrayOrdered = enumerable
                .OrderBy(x => x.Key)
                .ToArray();

            if (arrayOrdered.Length == 0)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            states = arrayOrdered
                .Select(x => x.Value)
                .ToArray();


            /*
            * Поиск индексов дял типов состояний!
            * pattern state
            */
            Initialize(arrayOrdered);
        }

        private void Initialize(KeyValuePair<RuntimeStateId, IRuntimeState>[] arrayOrdered)
        {
            var index = 0;
            var state = RuntimeStateType.START;

            foreach (var pair in arrayOrdered)
            {
                switch (state)
                {
                    case RuntimeStateType.START:
                        if (pair.Key.StateType == RuntimeStateType.START)
                        {
                            Index_start = index;
                            state = RuntimeStateType.INVOKE;
                        }
                        break;
                    case RuntimeStateType.INVOKE:
                        if (pair.Key.StateType == RuntimeStateType.INVOKE)
                        {
                            Index_invoke = index;
                            state = RuntimeStateType.MOVE;
                        }
                        break;
                    case RuntimeStateType.MOVE:
                        if (pair.Key.StateType == RuntimeStateType.MOVE)
                        {
                            Index_move = index;
                            state = RuntimeStateType.END;
                        }
                        break;
                    case RuntimeStateType.END:
                        if (pair.Key.StateType == RuntimeStateType.END)
                        {
                            Index_end = index;
                            state++;
                        }
                        break;
                    default:
                        return;
                }

                index++;
            }
        }

        public IRuntimeState this[int index]
        {
            get => states[index];
        }

        public int Index_start { get; private set; }
        public int Index_invoke { get; private set; }
        public int Index_move { get; private set; }
        public int Index_end { get; private set; }
        public int Count => states.Length;

        public IRuntimeStateEnumerator GetEnumerator()
        {
            return new MvcRuntimeStateEnumerator(this);
        }

    }
}
