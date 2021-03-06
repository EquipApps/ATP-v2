﻿using EquipApps.Mvc.Abstractions;
using System;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Дескриптер действия
    /// </summary>
    public abstract partial class ActionDescriptor
    {
        public ActionDescriptor(ActionDescriptorObject testCase, ActionDescriptorObject testStep)
        {
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
            TestStep = testStep ?? throw new ArgumentNullException(nameof(testStep));

            Id = Guid.NewGuid().ToString();
            
            OrderValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Properties  = new Dictionary<object, object>();
        }

        /// <summary>
        /// Gets an id which uniquely identifies the action.
        /// </summary>
        public string Id { get; }

        // <summary>
        /// Gets or sets the collection of order values that must be provided by ordering
        /// for the action to be selected.
        /// </summary>
        public IDictionary<string, string> OrderValues { get; set; }

        // <summary>
        /// Gets or sets the collection of route values that must be provided by routing
        /// for the action to be selected.
        /// </summary>
        public IDictionary<string, string> RouteValues { get; set; }

        /// <summary>
        /// A friendly name for this action.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Stores arbitrary metadata properties associated with the <see cref="ActionDescriptor"/>.
        /// </summary>
        public IDictionary<object, object> Properties { get; }

        //-------------------------------

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого случая
        /// </summary>
        public virtual ActionDescriptorObject TestCase { get; }

        /// <summary>
        /// Возвращает <see cref="ActionDescriptorObject"/> для Тесторого шага
        /// </summary>
        public virtual ActionDescriptorObject TestStep { get; }

        public virtual Number Number { get; set; }
    }
}
