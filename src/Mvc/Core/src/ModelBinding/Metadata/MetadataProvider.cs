using EquipApps.Mvc.ModelBinding.Metadata;
using NLib.AtpNetCore.Testing.Mvc.ModelBinding.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Metadata
{
    public class MetadataProvider : IMetadataProvider
    {
        private readonly TypeCache _typeCache = new TypeCache();
        private readonly Func<ModelMetadataIdentity, ModelMetadataCacheEntry> _cacheEntryFactory;
        private readonly ModelMetadataCacheEntry _metadataCacheEntryForObjectType;


        public MetadataProvider()
        {
            _cacheEntryFactory = CreateCacheEntry;
            _metadataCacheEntryForObjectType = GetMetadataCacheEntryForObjectType();
        }

        /// <inheritdoc />
        public ModelMetadata GetMetadataForType(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var cacheEntry = GetCacheEntry(modelType);

            return cacheEntry.Metadata;
        }

        /// <inheritdoc />
        public IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }


            var cacheEntry = GetCacheEntry(modelType);

            // We're relying on a safe race-condition for Properties - take care only
            // to set the value onces the properties are fully-initialized.
            if (cacheEntry.Details.Properties == null)
            {
                var key = ModelMetadataIdentity.ForType(modelType);
                var propertyDetails = CreatePropertyDetails(key);

                var properties = new ModelMetadata[propertyDetails.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    propertyDetails[i].ContainerMetadata = cacheEntry.Metadata;
                    properties[i] = CreateModelMetadata(propertyDetails[i]);
                }

                cacheEntry.Details.Properties = properties;
            }

            return cacheEntry.Details.Properties;
        }

        private ModelMetadataCacheEntry GetCacheEntry(Type modelType)
        {
            ModelMetadataCacheEntry cacheEntry;

            // Perf: We cached model metadata cache entry for "object" type to save ConcurrentDictionary lookups.
            if (modelType == typeof(object))
            {
                cacheEntry = _metadataCacheEntryForObjectType;
            }
            else
            {
                var key = ModelMetadataIdentity.ForType(modelType);
                cacheEntry = _typeCache.GetOrAdd(key, _cacheEntryFactory);
            }

            return cacheEntry;
        }

        #region Factory ModelMetadataCacheEntry

        private ModelMetadataCacheEntry GetMetadataCacheEntryForObjectType()
        {
            var key = ModelMetadataIdentity.ForType(typeof(object));
            var entry = CreateCacheEntry(key);
            return entry;
        }

        private ModelMetadataCacheEntry CreateCacheEntry(ModelMetadataIdentity key)
        {
            var details = CreateTypeDetails(key);
            var metadata = CreateModelMetadata(details);
            return new ModelMetadataCacheEntry(metadata, details);
        }

        #endregion

        #region Factory ModelMetadata

        protected virtual ModelMetadata CreateModelMetadata(DefaultModelDetails entry)
        {
            return new DefaultModelMetadata(this, entry);
        }

        #endregion

        #region Factory fo DefaultMetadataDetails

        protected virtual DefaultModelDetails CreateTypeDetails(ModelMetadataIdentity key)
        {
            return new DefaultModelDetails(key, ModelAttributes.GetAttributesForType(key.ModelType));
        }

        protected virtual DefaultModelDetails[] CreatePropertyDetails(ModelMetadataIdentity key)
        {
            var propertyHelpers = PropertyHelper.GetVisibleProperties(key.ModelType);

            var propertyEntries = new List<DefaultModelDetails>(propertyHelpers.Length);
            for (var i = 0; i < propertyHelpers.Length; i++)
            {
                var propertyHelper = propertyHelpers[i];
                var propertyKey = ModelMetadataIdentity.ForProperty(
                    propertyHelper.Property.PropertyType,
                    propertyHelper.Name,
                    key.ModelType);

                var attributes = ModelAttributes.GetAttributesForProperty(
                    key.ModelType,
                    propertyHelper.Property);

                var propertyEntry = new DefaultModelDetails(propertyKey, attributes);

                //--- GetMethod!
                //if (propertyHelper.Property.CanRead &&
                //    propertyHelper.Property.GetMethod?.IsPublic == true)
                //{
                //    propertyEntry.PropertyGetter = propertyHelper.ValueGetter;
                //}

                //--- SetMethod!
                //if (propertyHelper.Property.CanWrite &&
                //    propertyHelper.Property.SetMethod?.IsPublic == true &&
                //    !key.ModelType.GetTypeInfo().IsValueType)
                //{
                //    propertyEntry.PropertySetter = propertyHelper.ValueSetter;
                //}

                propertyEntries.Add(propertyEntry);
            }

            return propertyEntries.ToArray();
        }

        #endregion

        private class TypeCache : ConcurrentDictionary<ModelMetadataIdentity, ModelMetadataCacheEntry>
        {
            public TypeCache()
                : base(ModelMetadataIdentityComparer.Instance)
            {
            }
        }

        private struct ModelMetadataCacheEntry
        {
            public ModelMetadataCacheEntry(ModelMetadata metadata, DefaultModelDetails details)
            {
                Metadata = metadata;
                Details = details;
            }

            public ModelMetadata Metadata { get; }

            public DefaultModelDetails Details { get; }
        }

        private class ModelMetadataIdentityComparer : IEqualityComparer<ModelMetadataIdentity>
        {
            public static readonly ModelMetadataIdentityComparer Instance = new ModelMetadataIdentityComparer();

            public bool Equals(ModelMetadataIdentity x, ModelMetadataIdentity y)
            {
                return
                    x.ContainerType == y.ContainerType &&
                    x.ModelType == y.ModelType &&
                    x.Name == y.Name;
            }

            public int GetHashCode(ModelMetadataIdentity obj)
            {
                var hash = 17;
                hash = hash * 23 + obj.ModelType.GetHashCode();

                if (obj.ContainerType != null)
                {
                    hash = hash * 23 + obj.ContainerType.GetHashCode();
                }

                if (obj.Name != null)
                {
                    hash = hash * 23 + obj.Name.GetHashCode();
                }

                return hash;
            }
        }
    }
}
