using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class TransientServiceAttribute<T> : Attribute, IService where T : class
{
    public ServiceLifetime Lifetime => ServiceLifetime.Transient;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class TransientServiceAttribute : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Transient;
}
