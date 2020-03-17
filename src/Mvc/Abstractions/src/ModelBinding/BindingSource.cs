using EquipApps.Mvc.Abstractions;
using System;

namespace EquipApps.Mvc.ModelBinding
{
    /// <summary>
    /// Источник привязки
    /// </summary>   
    public class BindingSource : IEquatable<BindingSource>
    {
        /// <summary>
        /// Источник - <see cref="IModelProvider"/>
        /// </summary>
        public static readonly BindingSource ModelProvider = new BindingSource("ModelProvider");

        /// <summary>
        /// Источник - <see cref="ActionDescriptor"/> 
        /// </summary>
        public static readonly BindingSource DataText = new BindingSource("DataText");

        /// <summary>
        /// Источник - <see cref="ActionDescriptor.DataContexts"/> 
        /// </summary>
        public static readonly BindingSource DataContext = new BindingSource("DataContext");

        /// <summary>
        /// Источник - <see cref="BindingInfo.BinderType"/>
        /// </summary>
        public static readonly BindingSource Custom = new BindingSource(
           "Custom");



        public BindingSource(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }

        /// <summary>
        /// Возвращает уникальный идентификатор источника
        /// </summary>
        public string Id { get; }

        //--
        public virtual bool CanAcceptDataFrom(BindingSource bindingSource)
        {
            if (bindingSource == null)
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }

            return this == bindingSource;
        }

        //--
        public bool Equals(BindingSource other)
        {
            return string.Equals(other?.Id, Id, StringComparison.Ordinal);
        }

        //--
        public override bool Equals(object obj)
        {
            return Equals(obj as BindingSource);
        }

        //--
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        //--
        public static bool operator ==(BindingSource s1, BindingSource s2)
        {
            if (ReferenceEquals(s1, null))
            {
                return ReferenceEquals(s2, null);
            }

            return s1.Equals(s2);
        }

        //--
        public static bool operator !=(BindingSource s1, BindingSource s2)
        {
            return !(s1 == s2);
        }
    }
}
