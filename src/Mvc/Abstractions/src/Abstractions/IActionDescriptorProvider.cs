namespace EquipApps.Mvc.Abstractions
{
    /// <summary>
    /// Defines a contract for specifying <see cref="ActionDescriptor"/> instances.
    /// </summary>
    public interface IActionDescriptorProvider
    {
        int Order { get; }

        /// <summary>
        /// Called to execute the provider. 
        /// </summary>
        void OnProvidersExecuting(ActionDescriptorProviderContext context);

        /// <summary>
        /// Called to execute the provider, after the <see cref="OnProvidersExecuting"/> methods of all providers, have been called.
        /// </summary>
        void OnProvidersExecuted(ActionDescriptorProviderContext context);
    }
}
