using EquipApps.Testing;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Runtime
{
    public abstract class RuntimeContext
    {
        /// <summary>
        /// Флаг выхода из конвеера обработки
        /// </summary>
        public bool Handled { get; set; } = false;

        /// <summary>
        /// Возвращает <see cref="Testing.TestContext"/>
        /// </summary>
        public abstract TestContext TestContext { get; }

        /// <summary>
        /// Возвращает <see cref="IRuntimeStateEnumerator"/>
        /// </summary>
        public abstract IRuntimeStateEnumerator StateEnumerator { get; }














        //-----------------------------------------------



        /* Unmerged change from project 'AtpNetCore.Mvc.Abstractions (netcoreapp3.1)'
        Before:
                public abstract IRuntimeEnumerator  Enumerator { get; }



                public abstract IActionInvokerFactory Factory { get; }
        After:
                public abstract IRuntimeEnumerator  Enumerator { get; }



                public abstract IActionInvokerFactory Factory { get; }
        */
        public abstract IRuntimeEnumerator Enumerator { get; }



        public abstract IActionInvokerFactory Factory { get; }

        //-----------------------------------------------





        //-----------------------------------------------

        /// <summary>
        /// Переход на <see cref="RuntimeStateType"/>.
        /// </summary>
        /// 
        /// <param name="stateType">
        /// Состояние в которое произойдет переход
        /// </param>
        public void JumpTo(RuntimeStateType stateType)
        {
            if (!StateEnumerator.JumpTo(stateType))
            {
                throw new InvalidOperationException(nameof(StateEnumerator.JumpTo));
            }
        }


        //-----------------------------------------------

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();
    }
}
