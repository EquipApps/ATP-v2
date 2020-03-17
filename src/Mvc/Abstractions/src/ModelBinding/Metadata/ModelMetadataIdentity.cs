using System;

namespace EquipApps.Mvc.ModelBinding.Metadata
{
    public struct ModelMetadataIdentity : IEquatable<ModelMetadataIdentity>
    {
        /// <summary>
        /// Creates a <see cref="ModelMetadataIdentity"/> for the provided model <see cref="Type"/>.
        /// </summary>
        /// <param name="modelType">The model <see cref="Type"/>.</param>
        /// <returns>A <see cref="ModelMetadataIdentity"/>.</returns>
        public static ModelMetadataIdentity ForType(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            return new ModelMetadataIdentity()
            {
                ModelType = modelType,
            };
        }

        /// <summary>
        /// Creates a <see cref="ModelMetadataIdentity"/> for the provided property.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="containerType">The container type of the model property.</param>
        /// <returns>A <see cref="ModelMetadataIdentity"/>.</returns>
        public static ModelMetadataIdentity ForProperty(
            Type modelType,
            string name,
            Type containerType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            if (containerType == null)
            {
                throw new ArgumentNullException(nameof(containerType));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }

            return new ModelMetadataIdentity()
            {
                ModelType = modelType,
                Name = name,
                ContainerType = containerType,
            };
        }

        /// <summary>
        /// Gets the <see cref="Type"/> defining the model property respresented by the current
        /// instance, or <c>null</c> if the current instance does not represent a property.
        /// </summary>
        public Type ContainerType { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> represented by the current instance.
        /// </summary>
        public Type ModelType { get; private set; }

        /// <summary>
        /// Gets a value indicating the kind of metadata represented by the current instance.
        /// </summary>
        public ModelMetadataKind MetadataKind
        {
            get
            {
                if (ContainerType != null && Name != null)
                {
                    return ModelMetadataKind.Property;
                }
                else
                {
                    return ModelMetadataKind.Type;
                }
            }
        }

        /// <summary>
        /// Gets the name of the current instance if it represents a parameter or property, or <c>null</c> if
        /// the current instance represents a type.
        /// </summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public bool Equals(ModelMetadataIdentity other)
        {
            return
                ContainerType == other.ContainerType &&
                ModelType == other.ModelType &&
                Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = obj as ModelMetadataIdentity?;
            return other.HasValue && Equals(other.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ModelType.GetHashCode();

            if (ContainerType != null)
            {
                hash = hash * 23 + ContainerType.GetHashCode();
            }

            if (Name != null)
            {
                hash = hash * 23 + Name.GetHashCode();
            }

            return hash;
        }
    }
}
