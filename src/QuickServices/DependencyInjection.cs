using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuickServices;

public static class DependencyInjection
{
    public static IServiceCollection AddQuickServices(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
        {
            RegisterByAttribute(services, type, typeof(TransientServiceAttribute), typeof(TransientServiceAttribute<>), services.AddTransient);
            RegisterByAttribute(services, type, typeof(ScopedServiceAttribute), typeof(ScopedServiceAttribute<>), services.AddScoped);
            RegisterByAttribute(services, type, typeof(SingletonServiceAttribute), typeof(SingletonServiceAttribute<>), services.AddSingleton);

            RegisterKeyedByAttribute(services, type, typeof(KeyedTransientServiceAttribute), typeof(KeyedTransientServiceAttribute<>), services.AddKeyedTransient);
            RegisterKeyedByAttribute(services, type, typeof(KeyedScopedServiceAttribute), typeof(KeyedScopedServiceAttribute<>), services.AddKeyedScoped);
            RegisterKeyedByAttribute(services, type, typeof(KeyedSingletonServiceAttribute), typeof(KeyedSingletonServiceAttribute<>), services.AddKeyedSingleton);
        }

        return services;
    }

    private static void RegisterKeyedByAttribute(
        IServiceCollection services,
        Type type,
        Type attributeType,
        Type genericAttributeType,
        Func<Type, object?, Type, IServiceCollection> register)
    {
        foreach (var attr in type.GetCustomAttributes(false)
            .Where(a => a.GetType() == attributeType ||
                       (a.GetType().IsGenericType && a.GetType().GetGenericTypeDefinition() == genericAttributeType)))
        {
            var serviceType = attr.GetType().IsGenericType ? attr.GetType().GetGenericArguments()[0] : type;
            var serviceKey = attr.GetType().GetProperty(nameof(IKeyedService.ServiceKey))!.GetValue(attr);
            register(serviceType, serviceKey, type);
        }
    }

    private static void RegisterByAttribute(
        IServiceCollection services,
        Type type,
        Type attributeType,
        Type genericAttributeType,
        Func<Type, Type, IServiceCollection> register)
    {
        foreach (var attr in type.GetCustomAttributes(false)
            .Where(a => a.GetType() == attributeType ||
                       (a.GetType().IsGenericType && a.GetType().GetGenericTypeDefinition() == genericAttributeType)))
        {
            var serviceType = attr.GetType().IsGenericType ? attr.GetType().GetGenericArguments()[0] : type;
            register(serviceType, type);
        }
    }
}
