﻿using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Attribute
{
    /// <summary>
    /// Маркер.
    /// </summary>
    public interface IModelTypeMetadata
    {
        Type ModelType { get; }
    }
}