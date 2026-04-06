using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedScopedServiceAttribute<T>(object serviceKey) : Attribute, IKeyedService where T : class
{
    public object ServiceKey { get; } = serviceKey;

    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedScopedServiceAttribute(object serviceKey) : Attribute, IKeyedService
{
    public object ServiceKey { get; } = serviceKey;
    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
}
