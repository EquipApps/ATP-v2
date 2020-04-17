using EquipApps.Mvc.Abstractions;
using System;
using System.Reactive.Subjects;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Дескриптер действия
    /// </summary>
    public abstract partial class ActionDescriptor
    {

        public ActionDescriptor(ActionDescriptorObject testCase, ActionDescriptorObject testStep)
            :this()
        {
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
            TestStep = testStep ?? throw new ArgumentNullException(nameof(testStep));

            
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
    }
}
