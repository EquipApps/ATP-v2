// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// An attribute that enables binding for all properties the decorated controller model defines.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BindPropertiesAttribute : Attribute
    {
    }
}
