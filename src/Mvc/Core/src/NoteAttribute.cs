using System;
using System.Collections.Generic;
using System.Text;

namespace NLib.AtpNetCore.Mvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class NoteAttribute : Attribute
    {
        public NoteAttribute(string note)
        {
            //TODO: Реазиловать заметки
        }
    }
}
