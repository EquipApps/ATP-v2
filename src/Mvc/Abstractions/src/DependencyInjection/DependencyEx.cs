using EquipApps.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcDependencyEx
    {
        //-- AddActionDescriptorProvider
        public static void AddTransientActionDescriptorProvider<TProvider>(this IServiceCollection serviceDescriptors)
            where TProvider : class, IActionDescriptorProvider
        {
            //
            // Action Descriptor Provider
            // Transient
            // 
            serviceDescriptors.TryAddEnumerable(
                ServiceDescriptor.Transient<IActionDescriptorProvider, TProvider>());
        }

        //-- AddActionInvokerProvider
        public static void AddTransientActionInvokerProvider<TProvider>(this IServiceCollection serviceDescriptors)
            where TProvider : class, IActionInvokerProvider
        {
            //
            // Action Invoker Provider
            // Transient
            // 
            serviceDescriptors.TryAddEnumerable(
                ServiceDescriptor.Transient<IActionInvokerProvider, TProvider>());
        }
    }
}
