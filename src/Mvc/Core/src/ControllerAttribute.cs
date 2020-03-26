using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Указывает, что данный класс является контроллером.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ControllerAttribute : Attribute
    {

    }
}
