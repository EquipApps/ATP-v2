using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Properties
{
    public class IndexHelper
    {
        // Using an array rather than IEnumerable, as target will be called on the hot path numerous times.
        private static readonly ConcurrentDictionary<Type, IndexHelper[]> IndicesCache =
            new ConcurrentDictionary<Type, IndexHelper[]>();

        private static readonly ConcurrentDictionary<Type, IndexHelper[]> VisibleIndicesCache =
            new ConcurrentDictionary<Type, IndexHelper[]>();

        // We need to be able to check if a type is a 'ref struct' - but we need to be able to compile
        // for platforms where the attribute is not defined, like net46. So we can fetch the attribute
        // by late binding. If the attribute isn't defined, then we assume we won't encounter any
        // 'ref struct' types.
        private static readonly Type IsByRefLikeAttribute = Type.GetType("System.Runtime.CompilerServices.IsByRefLikeAttribute", throwOnError: false);

        public IndexHelper(PropertyInfo property)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Name = property.Name;
            Parameters = property.GetIndexParameters();
        }


        public virtual string Name { get; protected set; }

        public PropertyInfo Property { get; }

        public ParameterInfo[] Parameters { get; }


        private static IndexHelper CreateInstance(PropertyInfo property)
        {
            return new IndexHelper(property);
        }

        public static IndexHelper[] GetIndices(TypeInfo typeInfo)
        {
            return GetIndices(typeInfo.AsType());
        }

        public static IndexHelper[] GetIndices(Type type)
        {
            return GetIndices(type, p => CreateInstance(p), IndicesCache);
        }

        public static IndexHelper[] GetVisibleIndices(TypeInfo typeInfo)
        {
            return GetVisibleIndices(typeInfo.AsType(), p => CreateInstance(p), IndicesCache, VisibleIndicesCache);
        }

        public static IndexHelper[] GetVisibleIndices(Type type)
        {
            return GetVisibleIndices(type, p => CreateInstance(p), IndicesCache, VisibleIndicesCache);
        }

        protected static IndexHelper[] GetVisibleIndices(
           Type type,
           Func<PropertyInfo, IndexHelper> createIndexHelper,
           ConcurrentDictionary<Type, IndexHelper[]> allIndicesCache,
           ConcurrentDictionary<Type, IndexHelper[]> visibleIndicesCache)
        {
            if (visibleIndicesCache.TryGetValue(type, out var result))
            {
                return result;
            }

            // The simple and common case, this is normal POCO object - no need to allocate.
            var allIndicesDefinedOnType = true;
            var allIndices = GetIndices(type, createIndexHelper, allIndicesCache);
            foreach (var indexHelper in allIndices)
            {
                if (indexHelper.Property.DeclaringType != type)
                {
                    allIndicesDefinedOnType = false;
                    break;
                }
            }

            if (allIndicesDefinedOnType)
            {
                result = allIndices;
                visibleIndicesCache.TryAdd(type, result);
                return result;
            }

            // There's some inherited properties here, so we need to check for hiding via 'new'.
            var filteredIndices = new List<IndexHelper>(allIndices.Length);
            foreach (var indexHelper in allIndices)
            {
                var declaringType = indexHelper.Property.DeclaringType;
                if (declaringType == type)
                {
                    filteredIndices.Add(indexHelper);
                    continue;
                }

                // If this property was declared on a base type then look for the definition closest to the
                // the type to see if we should include it.
                var ignoreProperty = false;

                // Walk up the hierarchy until we find the type that actually declares this
                // PropertyInfo.
                var currentTypeInfo = type.GetTypeInfo();
                var declaringTypeInfo = declaringType.GetTypeInfo();
                while (currentTypeInfo != null && currentTypeInfo != declaringTypeInfo)
                {
                    // We've found a 'more proximal' public definition
                    var declaredProperty = currentTypeInfo.GetDeclaredProperty(indexHelper.Name);
                    if (declaredProperty != null)
                    {
                        ignoreProperty = true;
                        break;
                    }

                    currentTypeInfo = currentTypeInfo.BaseType?.GetTypeInfo();
                }

                if (!ignoreProperty)
                {
                    filteredIndices.Add(indexHelper);
                }
            }

            result = filteredIndices.ToArray();
            visibleIndicesCache.TryAdd(type, result);
            return result;
        }


        protected static IndexHelper[] GetIndices(
            Type type,
            Func<PropertyInfo, IndexHelper> createIndexHelper,
            ConcurrentDictionary<Type, IndexHelper[]> cache)
        {


            // Unwrap nullable types. This means Nullable<T>.Value and Nullable<T>.HasValue will not be
            // part of the sequence of properties returned by this method.
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (!cache.TryGetValue(type, out var helpers))
            {
                var properties = type.GetRuntimeProperties().Where(p => IsInterestingProperty(p));

                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsInterface)
                {
                    // Reflection does not return information about inherited properties on the interface itself.
                    properties = properties.Concat(typeInfo.ImplementedInterfaces.SelectMany(
                        interfaceType => interfaceType.GetRuntimeProperties().Where(p => IsInterestingProperty(p))));
                }

                helpers = properties.Select(p => createIndexHelper(p)).ToArray();
                cache.TryAdd(type, helpers);
            }

            return helpers;
        }




        private static bool IsInterestingProperty(PropertyInfo property)
        {
            return
                property.GetMethod != null &&
                property.GetMethod.IsPublic &&
                !property.GetMethod.IsStatic &&

                // PropertyHelper can't work with ref structs.
                !IsRefStructProperty(property) &&

                // Indexed properties
                property.GetMethod.GetParameters().Length != 0;
        }

        // PropertyHelper can't really interact with ref-struct properties since they can't be 
        // boxed and can't be used as generic types. We just ignore them.
        //
        // see: https://github.com/aspnet/Mvc/issues/8545
        private static bool IsRefStructProperty(PropertyInfo property)
        {
            return
                IsByRefLikeAttribute != null &&
                property.PropertyType.IsValueType &&
                property.PropertyType.IsDefined(IsByRefLikeAttribute);
        }
    }
}
