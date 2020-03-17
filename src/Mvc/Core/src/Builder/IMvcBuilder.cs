using EquipApps.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace NLib.AtpNetCore.Testing.Mvc.Infrastructure
{
    public interface IMvcBuilder
    {
        IServiceCollection Services { get; }

        ApplicationPartManager AppPartManager { get; }
    }
}
