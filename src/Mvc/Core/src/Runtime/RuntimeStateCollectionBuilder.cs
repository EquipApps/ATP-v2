using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquipApps.Mvc.Runtime
{
    public static class RuntimeStateCollectionBuilder
    {
        public static RuntimeStateCollection Build(IEnumerable<KeyValuePair<RuntimeStateId, IRuntimeState>> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            //-- Сортируем по ключу
            var arrayOrdered = enumerable
               .OrderBy(x => x.Key)
               .ToArray();

            if (arrayOrdered.Length == 0)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            //-- Поиск индексов
            var index_start     = 0;
            var index_invoke    = 0;
            var index_move      = 0;
            var index_end       = 0;

            var counter         = 0;
            var state           = RuntimeStateType.START;
            var end             = false;

            foreach (var pair in arrayOrdered)
            {
                if (end) 
                    break;

                switch (state)
                {
                    case RuntimeStateType.START:
                        if (pair.Key.StateType == RuntimeStateType.START)
                        {
                            index_start = counter;
                            state = RuntimeStateType.INVOKE;
                        }
                        break;
                    case RuntimeStateType.INVOKE:
                        if (pair.Key.StateType == RuntimeStateType.INVOKE)
                        {
                            index_invoke = counter;
                            state = RuntimeStateType.MOVE;
                        }
                        break;
                    case RuntimeStateType.MOVE:
                        if (pair.Key.StateType == RuntimeStateType.MOVE)
                        {
                            index_move = counter;
                            state = RuntimeStateType.END;
                        }
                        break;
                    case RuntimeStateType.END:
                        if (pair.Key.StateType == RuntimeStateType.END)
                        {
                            index_end = counter;
                            state++;
                        }
                        break;
                    default:
                        end = true;
                        break;                      
                }

                counter++;
            }


            var arrayStates = arrayOrdered
                .Select(x => x.Value)
                .ToArray();

            return new RuntimeStateCollection(arrayStates, index_start, index_invoke, index_move, index_end);
        }
    }
}
