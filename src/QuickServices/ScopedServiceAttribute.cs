using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ScopedServiceAttribute<T> : Attribute, IService where T : class
{
    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ScopedServiceAttribute : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
}
