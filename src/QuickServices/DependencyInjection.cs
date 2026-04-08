using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuickServices;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickServices(this IServiceCollection services)
    {
        foreach (var assembly in GetReferencingAssemblies(Assembly.GetCallingAssembly()))
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
