using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Указывает, что данный класс является контроллером.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ControllerAttribute : Attribute
    {

    }
}
