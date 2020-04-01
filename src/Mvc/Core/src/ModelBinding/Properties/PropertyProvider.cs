using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Properties
{
    /// <summary>
    /// Реализация <see cref="IPropertyProvider"/>
    /// </summary>
    public class PropertyProvider : IPropertyProvider
    {
        private readonly ConcurrentDictionary<PropertyIdentity, PropertyEntery> _propertyCache
            = new ConcurrentDictionary<PropertyIdentity, PropertyEntery>(PropertyIdentityComparer.Instance);

        private readonly IModelMetadataProvider _metadataProvider;
        private readonly ILogger<PropertyProvider> _logger;



        public PropertyProvider(IModelMetadataProvider metadataProvider, ILogger<PropertyProvider> logger)
        {
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));

            _logger = logger ?? throw new ArgumentNullException(nameof(metadataProvider));
            _logger.LogTrace("ctr");
        }


        public bool TryGetModelProperty(Type sourceType, string properyPath, out PropertyEntery property)
        {
            var key = PropertyIdentity.For(sourceType, properyPath);

            if (_propertyCache.TryGetValue(key, out property))
            {
                return true;
            }

            var propertytems = PropertyParser.ParseProperyPath(key.ProperyPath);

            if (TryCreatePropertyFor(key, propertytems, out property))
                if (_propertyCache.TryAdd(key, property))
                {
                    return true;
                }

            return false;
        }


        private bool TryCreatePropertyFor(PropertyIdentity identity, IReadOnlyList<ProperyPathItem> properyItems, out PropertyEntery cacheEntry)
        {
            var sourceMetadata = _metadataProvider.GetMetadataForType(identity.SourceType);
            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var sourceExpression = (Expression)Expression.ConvertChecked(sourceParameter, identity.SourceType);

            foreach (var properyItem in properyItems)
            {
                if (!TryGetExpressionForProperyItem(properyItem, ref sourceMetadata, ref sourceExpression))
                    break;
            }

            if (sourceMetadata == null || sourceMetadata == null)
            {
                cacheEntry = default;
                return false;
            }

            var castsourceCall = Expression.Convert(sourceExpression, typeof(object));
            var lambda = Expression.Lambda<PropertyExtractor>(castsourceCall, sourceParameter);
            var function = lambda.Compile();

            cacheEntry = new PropertyEntery(function, sourceMetadata.ModelType);

            return true;
        }

        private bool TryGetExpressionForProperyItem(ProperyPathItem properyItem, ref ModelMetadata metadata, ref Expression expression)
        {
            if (properyItem.IsIndex)
                return TryGetExpressionForProperyItemAsIndex(properyItem, ref metadata, ref expression);
            else
                return TryGetExpressionForProperyItemAsProperty(properyItem, ref metadata, ref expression);

        }

        private bool TryGetExpressionForProperyItemAsIndex(ProperyPathItem properyItem, ref ModelMetadata metadata, ref Expression expression)
        {
            var lMeta = metadata;

            if (lMeta.IsCollectionType)
            {
                //TODO: Разобраться что тут происходит
                var indexHelper = IndexHelper
                    .GetVisibleIndices(metadata.ModelType)
                    .Where(prop => prop.Parameters.Length == 1)
                    .Where(prop => prop.Parameters.All(prm => !lMeta.GetMetadataForType(prm.ParameterType).IsComplexType))
                    .FirstOrDefault();

                if (indexHelper != null)
                {
                    var parameter = indexHelper.Parameters[0];
                    var parameterType = parameter.ParameterType;
                    var parameterValue = TypeDescriptor.GetConverter(parameterType).ConvertFromString(properyItem.Value);
                    var parameterExpression = Expression.Constant(parameterValue, parameterType);

                    expression = Expression.Property(expression, indexHelper.Property, parameterExpression);
                    metadata = metadata.GetMetadataForType(metadata.ElementType);
                    return true;
                }
            }

            if (lMeta.ModelType.IsArray)
            {
                var parameterType = typeof(int);
                var parameterValue = TypeDescriptor.GetConverter(parameterType).ConvertFromString(properyItem.Value);
                var parameterExpression = Expression.Constant(parameterValue, parameterType);

                expression = Expression.ArrayIndex(expression, parameterExpression);
                metadata = metadata.GetMetadataForType(metadata.ElementType);
                return true;
            }


            _logger.LogWarning("TryGetExpressionForProperyItemAsIndex не ожиданный случай!");
            return false;


        }

        private bool TryGetExpressionForProperyItemAsProperty(ProperyPathItem properyItem, ref ModelMetadata metadata, ref Expression expression)
        {
            //-- Извлекаем свойство!
            metadata = metadata.Properties[properyItem.Value];

            //-- Если свойтва нет, то создать не можем!
            if (metadata == null)
                return false;

            expression = Expression.Property(expression, metadata.PropertyName);

            return true;
        }


        //TODO: Когданибудь разобраться с хеш ключом
        private struct PropertyIdentity : IEquatable<PropertyIdentity>
        {
            public static PropertyIdentity For(Type sourceType, string properyPath)
            {
                if (sourceType == null)
                {
                    throw new ArgumentNullException(nameof(sourceType));
                }

                if (string.IsNullOrEmpty(properyPath))
                {
                    throw new ArgumentException(nameof(properyPath));
                }

                return new PropertyIdentity()
                {
                    SourceType = sourceType,
                    ProperyPath = properyPath,
                };
            }

            /// <summary>
            /// Возвращает тип источника
            /// </summary>
            public Type SourceType { get; set; }

            /// <summary>
            /// Возвращает путь к свойству
            /// </summary>
            public string ProperyPath { get; set; }

            public bool Equals(PropertyIdentity other)
            {
                return
                   SourceType == other.SourceType &&
                   ProperyPath == other.ProperyPath;
            }

            public override bool Equals(object obj)
            {
                var other = obj as PropertyIdentity?;
                return other.HasValue && Equals(other.Value);
            }

            public override int GetHashCode()
            {
                var hash = 17;
                hash = hash * 23 + SourceType.GetHashCode();


                if (ProperyPath != null)
                {
                    hash = hash * 23 + ProperyPath.GetHashCode();
                }

                return hash;
            }
        }

        private class PropertyIdentityComparer : IEqualityComparer<PropertyIdentity>
        {
            public static readonly PropertyIdentityComparer Instance = new PropertyIdentityComparer();

            public bool Equals(PropertyIdentity x, PropertyIdentity y)
            {
                return
                    x.SourceType == y.SourceType &&
                    x.ProperyPath == y.ProperyPath;
            }

            public int GetHashCode(PropertyIdentity obj)
            {
                var hash = 17;
                hash = hash * 23 + obj.SourceType.GetHashCode();



                if (obj.ProperyPath != null)
                {
                    hash = hash * 23 + obj.ProperyPath.GetHashCode();
                }

                return hash;
            }
        }
    }
}
