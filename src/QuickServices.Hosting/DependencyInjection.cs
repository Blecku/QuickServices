using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace QuickServices.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickHostedServices(this IServiceCollection services)
    {
        foreach (var assembly in AssemblyScanner.GetReferencingAssemblies(Assembly.GetCallingAssembly(), typeof(DependencyInjection).Assembly))
            services.AddQuickHostedServices(assembly);

        return services;
    }

    public static IServiceCollection AddQuickHostedServices(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.GetCustomAttribute<HostedServiceAttribute>() is not null);

        foreach (var type in types)
        {
            if (!typeof(IHostedService).IsAssignableFrom(type))
                throw new InvalidOperationException(
                    $"Type '{type.FullName}' is marked with [HostedService] but does not implement IHostedService.");

            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), type));
        }

        return services;
    }
}
