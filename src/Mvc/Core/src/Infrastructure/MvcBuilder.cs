using EquipApps.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using NLib.AtpNetCore.Testing.Mvc.Infrastructure;
using System;

namespace EquipApps.Mvc.Infrastructure
{
    public class MvcBuilder : IMvcBuilder
    {
        public MvcBuilder(IServiceCollection services, ApplicationPartManager manager)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            AppPartManager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        /// <inheritdoc />
        public ApplicationPartManager AppPartManager { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
