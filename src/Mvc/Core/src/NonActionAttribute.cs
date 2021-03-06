﻿using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Маркер. Указывает, что метод не является действием
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class NonActionAttribute : Attribute
    {
    }
}
