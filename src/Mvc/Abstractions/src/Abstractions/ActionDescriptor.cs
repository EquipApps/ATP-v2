using EquipApps.Mvc.Abstractions;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Дескриптер действия
    /// </summary>
    public abstract partial class ActionDescriptor : IDisposable
    {
        public ActionDescriptor(ActionDescriptorObject testCase, ActionDescriptorObject testStep)
            :this()
        {
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
            TestStep = testStep ?? throw new ArgumentNullException(nameof(testStep));

          
            Exception = null;
            Result = Result.NotRun;
            State  = State.Empy;
        }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого случая
        /// </summary>
        public virtual ActionDescriptorObject TestCase { get; }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого шага
        /// </summary>
        public virtual ActionDescriptorObject TestStep { get; }

        //-------------------------------

        [Obsolete("User RouteValues")]
        public string Area
        {
            get
            {
                if (RouteValues.TryGetValue("Area", out string value))
                    return value;
                else
                    return null;
            }
        }
        public virtual Number Number { get; set; }

       

        /// <summary>
        /// Флаг. Указывает будет ли элемент участвовать в проверке.
        /// </summary>
        public bool IsCheck { get; set; } = true;

        /// <summary>
        /// Флаг. Точки остановки
        /// </summary>
        public bool IsBreak { get; set; } = false;

        /// <summary>
        /// Исключение (Ошибки)
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public Result Result { get; set; }
        

        /// <summary>
        /// Результат
        /// </summary>
        public State State { get; set; }




        public void Dispose()
        {

        }
    }
}
