using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ScopedServiceAttribute<T>(int registrationOrder = 1)
    : ScopedServiceAttribute(registrationOrder: registrationOrder) where T : class
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ScopedServiceAttribute(int registrationOrder = 1) : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
    public int RegistrationOrder { get; } = registrationOrder;
}
