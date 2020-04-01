using EquipApps.Mvc.ApplicationModels;
using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Metadata;
using System.Collections.Generic;

namespace EquipApps.Mvc
{
    public class MvcOptions
    {
        public MvcOptions()
        {
            Conventions = new List<IApplicationModelConvention>();



            BindingProviders = new List<IBinderProvider>();

            ModelBindingMessageProvider = new DefaultModelBindingMessageProvider();
            ModelMetadataDetailsProviders = new List<IMetadataDetailsProvider>();
        }






        /// <summary>
        /// Gets a list of <see cref="IApplicationModelConvention"/> instances that will be applied to
        /// the <see cref="ApplicationModel"/> when discovering actions.
        /// </summary>
        public IList<IApplicationModelConvention> Conventions { get; }








        /// <summary>
        /// Стартовый индекс умолчанию.
        /// </summary>
        public int StartIndex { get; set; } = 1;

        /// <summary>
        /// Возвращает смписок <see cref="IBinderProvider"/>
        /// </summary>
        public List<IBinderProvider> BindingProviders { get; }

        /// <summary>
        /// Gets the default <see cref="ModelBinding.Metadata.ModelBindingMessageProvider"/>. Changes here are copied to the
        /// <see cref="ModelMetadata.ModelBindingMessageProvider"/> property of all <see cref="ModelMetadata"/>
        /// instances unless overridden in a custom <see cref="IBindingMetadataProvider"/>.
        /// </summary>
        public DefaultModelBindingMessageProvider ModelBindingMessageProvider { get; internal set; }

        /// <summary>
        /// Gets a list of <see cref="IMetadataDetailsProvider"/> instances that will be used to
        /// create <see cref="ModelMetadata"/> instances.
        /// </summary>
        /// <remarks>
        /// A provider should implement one or more of the following interfaces, depending on what
        /// kind of details are provided:
        /// <ul>
        /// <li><see cref="IBindingMetadataProvider"/></li>
        /// <li><see cref="IDisplayMetadataProvider"/></li>
        /// <li><see cref="IValidationMetadataProvider"/></li>
        /// </ul>
        /// </remarks>
        public IList<IMetadataDetailsProvider> ModelMetadataDetailsProviders { get; }


        /// <summary>
        /// Gets or sets a value that determines if MVC will remove the suffix "Async" applied to
        /// controller action names.
        /// <para>
        /// <see cref="ControllerActionDescriptor.ActionName"/> is used to construct the route to the action as
        /// well as in view lookup. When <see langword="true"/>, MVC will trim the suffix "Async" applied
        /// to action method names.
        /// For example, the action name for <c>ProductsController.ListProductsAsync</c> will be
        /// canonicalized as <c>ListProducts.</c>. Consequently, it will be routeable at
        /// <c>/Products/ListProducts</c> with views looked up at <c>/Views/Products/ListProducts.cshtml</c>.
        /// </para>
        /// <para>
        /// This option does not affect values specified using using <see cref="ActionNameAttribute"/>.
        /// </para>
        /// </summary>
        /// <value>
        /// The default value is <see langword="true"/>.
        /// </value>
        public bool SuppressAsyncSuffixInActionNames { get; set; } = true;
        
    }
}
