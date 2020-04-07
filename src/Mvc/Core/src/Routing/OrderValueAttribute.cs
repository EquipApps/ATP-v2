// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace EquipApps.Mvc.Routing
{
    /// <summary>
    /// <para>
    /// An attribute which specifies a required route value for an action or controller.
    /// </para>
    /// <para>
    /// When placed on an action, the route data of a request must match the expectations of the required route data
    /// in order for the action to be selected. All other actions without a route value for the given key cannot be
    /// selected unless the route data of the request does omits a value matching the key.
    /// See <see cref="IRouteValueProvider"/> for more details and examples.
    /// </para>
    /// <para>
    /// When placed on a controller, unless overridden by the action, the constraint applies to all
    /// actions defined by the controller.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class OrderValueAttribute : Attribute, IOrderValueProvider
    {
        /// <summary>
        /// Creates a new <see cref="RouteValueAttribute"/>.
        /// </summary>
        /// <param name="orderKey">The route value key.</param>
        /// <param name="orderValue">The expected route value.</param>
        protected OrderValueAttribute(
            string orderKey,
            string orderValue)
        {
            if (orderKey == null)
            {
                throw new ArgumentNullException(nameof(orderKey));
            }

            if (orderValue == null)
            {
                throw new ArgumentNullException(nameof(orderValue));
            }

            OrderKey = orderKey;
            OrderValue = orderValue;
        }

        /// <inheritdoc />
        public string OrderKey { get; }

        /// <inheritdoc />
        public string OrderValue { get; }
    }
}
