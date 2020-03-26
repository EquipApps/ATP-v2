using EquipApps.Testing.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyEx
    {
        public static void AddScopedFeatureProvider<TProvider>(this IServiceCollection services)
            where TProvider : class, IFeatureProvider
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Scoped<IFeatureProvider, TProvider>());
        }

        public static void AddSingletonFeatureProvider<TProvider>(this IServiceCollection services)
            where TProvider : class, IFeatureProvider
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IFeatureProvider, TProvider>());
        }

        public static void AddTransientFeatureProvider<TProvider>(this IServiceCollection services)
            where TProvider : class, IFeatureProvider
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IFeatureProvider, TProvider>());
        }
    }
}
