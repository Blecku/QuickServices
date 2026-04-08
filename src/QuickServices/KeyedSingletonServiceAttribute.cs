using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedSingletonServiceAttribute<T>(object serviceKey) : Attribute, IKeyedService where T : class
{
    public object ServiceKey { get; } = serviceKey;

    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedSingletonServiceAttribute(object serviceKey) : Attribute, IKeyedService
{
    public object ServiceKey { get; } = serviceKey;

    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
}
