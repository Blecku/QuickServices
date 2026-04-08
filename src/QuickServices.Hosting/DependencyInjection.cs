using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace QuickServices.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickHostedServices(this IServiceCollection services)
    {
        foreach (var assembly in GetReferencingAssemblies(Assembly.GetCallingAssembly()))
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

    internal static HashSet<Assembly> GetReferencingAssemblies(Assembly root)
    {
        var quickServicesPrefix = typeof(DependencyInjection).Assembly.GetName().Name!;
        var visited = new HashSet<Assembly>();
        var stack = new Stack<Assembly>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (!visited.Add(current)) continue;

            foreach (var reference in current.GetReferencedAssemblies())
            {
                try
                {
                    var referenced = Assembly.Load(reference);
                    if (visited.Contains(referenced)) continue;

                    if (reference.Name == quickServicesPrefix
                        || referenced.GetReferencedAssemblies().Any(r => r.Name == quickServicesPrefix))
                        stack.Push(referenced);
                }
                catch { }
            }
        }

        visited.RemoveWhere(a => a.GetName().Name == quickServicesPrefix);
        return visited;
    }
}
