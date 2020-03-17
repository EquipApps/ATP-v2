using EquipApps.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Metadata
{
    public class DefaultModelMetadata : ModelMetadata
    {
        private IMetadataProvider _provider;
        private DefaultModelDetails _details;

        private ModelPropertyCollection _properties;

        public DefaultModelMetadata(IMetadataProvider provider, DefaultModelDetails details)
            : base(details.Key)
        {
            _provider = provider;
            _details = details;
        }


        //public DisplayMetadata DisplayMetadata
        //{
        //    get
        //    {
        //        if (_details.DisplayMetadata == null)
        //        {
        //            _details.DisplayMetadata = new DisplayMetadata();

        //            var metadataAttribute = _details.ModelAttributes.Attributes;
        //            var info = DisplayInfoBuilder.GetDisplayInfo(metadataAttribute);

        //            if (info != null)
        //            {
        //                _details.DisplayMetadata.Number = info.Number;
        //                _details.DisplayMetadata.Title = info.Title;
        //            }
        //        }

        //        return _details.DisplayMetadata;
        //    }
        //}

        public override ModelPropertyCollection Properties
        {
            get
            {
                if (_properties == null)
                {
                    var properties = _provider.GetMetadataForProperties(ModelType);
                    properties = properties.OrderBy(p => p.Order);

                    _properties = new ModelPropertyCollection(properties);
                }

                return _properties;
            }
        }


        public override int Order => 100;

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType)
        {
            return _provider.GetMetadataForProperties(modelType);
        }

        public override ModelMetadata GetMetadataForType(Type modelType)
        {
            return _provider.GetMetadataForType(modelType);
        }
    }
}
