using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedScopedServiceAttribute<T>(object serviceKey, int registrationOrder = 1)
    : KeyedScopedServiceAttribute(serviceKey, registrationOrder: registrationOrder) where T : class
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class KeyedScopedServiceAttribute(object serviceKey, int registrationOrder = 1) : Attribute, IKeyedService
{
    public object ServiceKey { get; } = serviceKey;
    public int RegistrationOrder { get; } = registrationOrder;
    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
}
