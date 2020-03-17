using System;

namespace EquipApps.Mvc.Runtime
{
    public sealed class RuntimeStateId : IComparable<RuntimeStateId>, IComparable
    {
        /// <summary>
        /// Состояние начала.
        /// </summary>
        public static RuntimeStateId Start { get; } = new RuntimeStateId(RuntimeStateType.START);

        /// <summary>
        /// Состояние выполнения.
        /// </summary>
        public static RuntimeStateId Invoke { get; } = new RuntimeStateId(RuntimeStateType.INVOKE);

        /// <summary>
        /// Состояние перемещения.
        /// </summary>
        public static RuntimeStateId Move { get; } = new RuntimeStateId(RuntimeStateType.MOVE);

        /// <summary>
        /// Состояние завершения
        /// </summary>
        public static RuntimeStateId End { get; } = new RuntimeStateId(RuntimeStateType.END);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtimeStateType">
        /// 
        /// </param>
        /// 
        /// <param name="order">
        /// 
        /// </param>
        private RuntimeStateId(RuntimeStateType runtimeStateType, int order = 0)
        {
            StateType = runtimeStateType;
            Order = order;
        }

        /// <summary>
        /// 
        /// </summary>
        public RuntimeStateType StateType { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; }

        public static RuntimeStateId operator +(RuntimeStateId left, int order)
        {
            return new RuntimeStateId(left.StateType, left.Order + order);
        }
        public static RuntimeStateId operator -(RuntimeStateId left, int order)
        {
            return new RuntimeStateId(left.StateType, left.Order - order);
        }

        public int CompareTo(RuntimeStateId other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            int currentState = (int)StateType;
            int otherState = (int)other.StateType;
            int result = currentState.CompareTo(otherState);

            if (result == 0)
            {
                result = Order.CompareTo(other.Order);
            }

            return result;
        }
        public int CompareTo(object obj)
        {
            return CompareTo(obj as RuntimeStateId);
        }
    }
}
