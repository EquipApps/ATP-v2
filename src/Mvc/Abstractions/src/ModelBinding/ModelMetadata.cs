using EquipApps.Mvc.ModelBinding.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EquipApps.Mvc.ModelBinding
{
    ///// <summary>
    ///// A metadata representation of a model type, property or parameter.
    ///// </summary>
    //[DebuggerDisplay("{DebuggerToString(),nq}")]
    //public abstract class ModelMetadata : IEquatable<ModelMetadata>, IModelMetadataProvider
    //{
    //    public static readonly int DefaultOrder = 10000;

    //    private int? _hashCode;

    //    protected ModelMetadata(ModelMetadataIdentity identity)
    //    {
    //        Identity = identity;

    //        InitializeTypeInformation();
    //    }

    //    /// <summary>
    //    /// Возвращает ключ
    //    /// </summary>
    //    protected ModelMetadataIdentity Identity { get; }

    //    //-------------------------------------------------------------

    //    public ModelMetadataKind MetadataKind => Identity.MetadataKind;

    //    //-------------------------------------------------------------

    //    /// <summary>
    //    /// Возвращает тип контейнера, если метаданные представляют свойство, иначе NULL
    //    /// </summary>
    //    public Type ContainerType => Identity.ContainerType;

    //    /// <summary>
    //    /// Возвращает методанные контейнера
    //    /// </summary>
    //    public virtual ModelMetadata ContainerMetadata
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    //-------------------------------------------------------------

    //    /// <summary>
    //    /// Возвращает тип модели
    //    /// </summary>
    //    public Type ModelType => Identity.ModelType;

    //    //-------------------------------------------------------------

    //    /// <summary>
    //    /// Возвращает название свойства или параметра!
    //    /// </summary>
    //    public string PropertyName => Identity.Name;

    //    //-------------------------------------------------------------

    //    /// <summary>
    //    /// Возвращает колекцию методанных свойств
    //    /// </summary>
    //    public abstract ModelPropertyCollection Properties { get; }


    //    //------------------------------------------------------------

    //    //- Есть конвертер!
    //    public bool IsComplexType { get; private set; }
    //    public bool IsNullableValueType { get; private set; }
    //    public bool IsReferenceOrNullableType { get; private set; }
    //    public Type UnderlyingOrModelType { get; private set; }
    //    public bool IsCollectionType { get; private set; }
    //    public bool IsEnumerableType { get; private set; }
    //    public Type ElementType { get; private set; }


    //    private void InitializeTypeInformation()
    //    {
    //        IsComplexType = !TypeDescriptor.GetConverter(ModelType).CanConvertFrom(typeof(string));
    //        IsNullableValueType = Nullable.GetUnderlyingType(ModelType) != null;
    //        IsReferenceOrNullableType = !ModelType.IsValueType || IsNullableValueType;
    //        UnderlyingOrModelType = Nullable.GetUnderlyingType(ModelType) ?? ModelType;

    //        IsCollectionType = ModelType.GetTypeInfo().ImplementedInterfaces.Any(x =>
    //           x.IsConstructedGenericType &&
    //           x.GetGenericTypeDefinition() == typeof(ICollection<>));

    //        if (ModelType == typeof(string) || !typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(ModelType.GetTypeInfo()))
    //        {
    //            // Do nothing, not Enumerable.
    //        }
    //        else if (ModelType.IsArray)
    //        {
    //            IsEnumerableType = true;
    //            ElementType = ModelType.GetElementType();
    //        }
    //        else
    //        {
    //            IsEnumerableType = true;

    //            var enumerableType = ModelType.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(x =>
    //                x.IsConstructedGenericType &&
    //                x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

    //            ElementType = enumerableType?.GenericTypeArguments[0];

    //            if (ElementType == null)
    //            {
    //                // ModelType implements IEnumerable but not IEnumerable<T>.
    //                ElementType = typeof(object);
    //            }
    //        }
    //    }

    //    //-----------------------------------------------------

    //    public bool Equals(ModelMetadata other)
    //    {
    //        if (ReferenceEquals(this, other))
    //        {
    //            return true;
    //        }

    //        if (other == null)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return Identity.Equals(other.Identity);
    //        }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return Equals(obj as ModelMetadata);
    //    }

    //    public override int GetHashCode()
    //    {
    //        // Normally caching the hashcode would be dangerous, but Identity is deeply immutable so this is safe.
    //        if (_hashCode == null)
    //        {
    //            _hashCode = Identity.GetHashCode();
    //        }

    //        return _hashCode.Value;
    //    }

    //    //-----------------------------------------------------

    //    public abstract int Order { get; }
    //    public abstract ModelMetadata GetMetadataForType(Type modelType);
    //    public abstract IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType);
    //}

    

}
