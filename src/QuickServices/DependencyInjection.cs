using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuickServices;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickServices(this IServiceCollection services)
    {
        var assemblies = AssemblyScanner.GetReferencingAssemblies(Assembly.GetCallingAssembly(), typeof(DependencyInjection).Assembly);
        return services.RegisterServices(assemblies);
    }

    public static IServiceCollection AddQuickServices(this IServiceCollection services, Assembly assembly)
    {
        return services.RegisterServices([assembly]);
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var registrations = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(type => type.GetCustomAttributes(false)
                .OfType<IService>()
                .Select(attr => (type, attr)))
            .OrderBy(r => r.attr.RegistrationOrder);

        foreach (var (type, attr) in registrations)
        {
            var serviceType = attr.GetType().IsGenericType ? attr.GetType().GetGenericArguments()[0] : type;

            switch (attr)
            {
                case IKeyedService keyed:
                    services.Add(new ServiceDescriptor(serviceType, keyed.ServiceKey, type, keyed.Lifetime));
                    break;
                default:
                    services.Add(new ServiceDescriptor(serviceType, type, attr.Lifetime));
                    break;
            }
        }

        return services;
    }
}
