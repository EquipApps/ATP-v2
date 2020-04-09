﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Infrastructure;

namespace EquipApps.Mvc.Controllers
{
    /// <summary>
    /// A descriptor for model bound properties of a controller.
    /// </summary>
    public class ControllerBoundPropertyDescriptor : ParameterDescriptor, IPropertyInfoParameterDescriptor
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.PropertyInfo"/> for this property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }
    }
}
