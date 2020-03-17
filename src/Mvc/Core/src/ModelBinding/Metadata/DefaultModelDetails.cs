using EquipApps.Mvc.ModelBinding.Metadata;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Metadata
{
    public class DefaultModelDetails
    {
        /// <summary>
        /// Creates a new <see cref="DefaultModelDetails"/>.
        /// </summary>
        /// <param name="key">The <see cref="ModelMetadataIdentity"/>.</param>
        /// <param name="attributes">The set of model attributes.</param>
        public DefaultModelDetails(ModelMetadataIdentity key, ModelAttributes attributes)
        {
            Key = key;
            ModelAttributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Gets or sets the <see cref="ModelMetadataIdentity"/>.
        /// </summary>
        public ModelMetadataIdentity Key { get; }

        /// <summary>
        /// Gets or sets the set of model attributes.
        /// </summary>
        public ModelAttributes ModelAttributes { get; }

        //------------------------------------------------------------

        /// <summary>
        /// Gets or sets the <see cref="Metadata.BindingMetadata"/>.
        /// </summary>
        //public BindingMetadata BindingMetadata { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Metadata.DisplayMetadata"/>.
        /// </summary>
        //public DisplayMetadata DisplayMetadata { get; set; }

        //------------------------------------------------------------

        /// <summary>
        /// Gets or sets the <see cref="ModelMetadata"/> of the container type.
        /// </summary>
        public ModelMetadata ContainerMetadata { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ModelMetadata"/> entries for the model properties.
        /// </summary>
        public ModelMetadata[] Properties { get; set; }

        //------------------------------------------------------------

        //public Func<object, object> PropertyGetter { get; set; }

        //public Action<object, object> PropertySetter { get; set; }
    }
}