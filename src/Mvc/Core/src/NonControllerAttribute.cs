using System;

namespace NLib.AtpNetCore.Mvc
{
    /// <summary>
    /// Указывает что данный класс не является контроллером.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonControllerAttribute : Attribute
    {
    }
}
