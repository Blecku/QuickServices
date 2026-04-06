using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

internal interface IService
{
    ServiceLifetime Lifetime { get; }
}
