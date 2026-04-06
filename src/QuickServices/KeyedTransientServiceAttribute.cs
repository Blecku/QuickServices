using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedTransientServiceAttribute<T>(object serviceKey) : Attribute, IKeyedService where T : class
{
    public object ServiceKey { get; } = serviceKey;

    public ServiceLifetime Lifetime => ServiceLifetime.Transient;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedTransientServiceAttribute(object serviceKey) : Attribute, IKeyedService
{
    public object ServiceKey { get; } = serviceKey;

    public ServiceLifetime Lifetime => ServiceLifetime.Transient;
}
