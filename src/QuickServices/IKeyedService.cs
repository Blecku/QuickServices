using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

internal interface IKeyedService
{
    object ServiceKey { get; }
    ServiceLifetime Lifetime { get; }
}
