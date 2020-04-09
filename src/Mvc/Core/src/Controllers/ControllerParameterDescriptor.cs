// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using EquipApps.Mvc.Abstractions;
using EquipApps.Mvc.Infrastructure;

namespace EquipApps.Mvc.Controllers
{
    /// <summary>
    /// A descriptor for method parameters of an action method.
    /// </summary>
    public class ControllerParameterDescriptor : ParameterDescriptor, IParameterInfoParameterDescriptor
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.ParameterInfo"/>.
        /// </summary>
        public ParameterInfo ParameterInfo { get; set; }
    }
}
