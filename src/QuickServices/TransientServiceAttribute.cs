using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class TransientServiceAttribute<T>(int registrationOrder = 1)
    : TransientServiceAttribute(registrationOrder: registrationOrder) where T : class
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class TransientServiceAttribute(int registrationOrder = 1) : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Transient;
    public int RegistrationOrder { get; } = registrationOrder;
}
