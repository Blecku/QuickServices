using Microsoft.Extensions.DependencyInjection;

namespace QuickServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class SingletonServiceAttribute<T> : Attribute, IService where T : class
{
    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class SingletonServiceAttribute : Attribute, IService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
}
