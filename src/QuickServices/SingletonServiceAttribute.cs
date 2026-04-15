using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class SingletonServiceAttribute<T>(int registrationOrder = 1)
    : SingletonServiceAttribute(registrationOrder: registrationOrder) where T : class
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class SingletonServiceAttribute(int registrationOrder = 1) : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
    public int RegistrationOrder { get; } = registrationOrder;
}
