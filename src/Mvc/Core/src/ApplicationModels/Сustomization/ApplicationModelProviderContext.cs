﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.Mvc.ApplicationModels.Сustomization
{
    /// <summary>
    /// A context object for <see cref="IApplicationModelProvider"/>.
    /// </summary>
    public class ApplicationModelProviderContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationModelProviderContext"/>.
        /// </summary>
        public ApplicationModelProviderContext(IEnumerable<TypeInfo> controllerTypes)
        {
            if (controllerTypes == null)
            {
                throw new ArgumentNullException(nameof(controllerTypes));
            }

            ControllerTypes = controllerTypes;
        }

        /// <summary>
        /// Gets the discovered controller <see cref="TypeInfo"/> instances.
        /// </summary>
        public IEnumerable<TypeInfo> ControllerTypes { get; }

        /// <summary>
        /// Gets the <see cref="ApplicationModel"/>.
        /// </summary>
        public ApplicationModel Result { get; } = new ApplicationModel();
    }
}