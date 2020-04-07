﻿using System;
using System.Collections.Generic;

namespace EquipApps.Mvc.Infrastructure
{
    /// <summary>
    /// A cached collection of <see cref="ActionDescriptor" />.
    /// </summary>
    public class ActionDescriptorCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDescriptorCollection"/>.
        /// </summary>
        /// <param name="items">The result of action discovery</param>
        /// <param name="version">The unique version of discovered actions.</param>
        public ActionDescriptorCollection(IReadOnlyList<ActionDescriptor> items, int version)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Items = items;
            Version = version;
        }

        /// <summary>
        /// Returns the cached <see cref="IReadOnlyList{ActionDescriptor}"/>.
        /// </summary>
        public IReadOnlyList<ActionDescriptor> Items { get; }

        /// <summary>
        /// Returns the unique version of the currently cached items.
        /// </summary>
        public int Version { get; }
    }
}