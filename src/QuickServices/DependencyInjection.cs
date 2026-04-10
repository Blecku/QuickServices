using Microsoft.Extensions.DependencyInjection;
using QuickServices.Core;
using System.Reflection;

namespace QuickServices;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickServices(this IServiceCollection services)
    {
        foreach (var assembly in AssemblyScanner.GetReferencingAssemblies(Assembly.GetCallingAssembly(), typeof(DependencyInjection).Assembly))
            services.AddQuickServices(assembly);

        return services;
    }

    public static IServiceCollection AddQuickServices(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
        {
            foreach (var attr in type.GetCustomAttributes(false))
            {
                var serviceType = attr.GetType().IsGenericType ? attr.GetType().GetGenericArguments()[0] : type;

                switch (attr)
                {
                    case IKeyedService keyed:
                        services.Add(new ServiceDescriptor(serviceType, keyed.ServiceKey, type, keyed.Lifetime));
                        break;
                    case IService service:
                        services.Add(new ServiceDescriptor(serviceType, type, service.Lifetime));
                        break;
                }
            }
        }

        return services;
    }
}
