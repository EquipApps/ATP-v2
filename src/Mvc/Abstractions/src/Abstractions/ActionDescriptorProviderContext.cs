﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// A context for <see cref="IActionDescriptorProvider"/>.
    /// </summary>
    public class ActionDescriptorProviderContext
    {
        /// <summary>
        /// Gets the <see cref="IList{T}" /> of <see cref="ActionDescriptor"/> instances of <see cref="IActionDescriptorProvider"/>
        /// can populate.
        /// </summary>
        public List<ActionDescriptor> Results { get; set; } = new List<ActionDescriptor>();
    }
}
