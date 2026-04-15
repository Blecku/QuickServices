# QuickServices

Attribute-based service registration for Microsoft.Extensions.DependencyInjection. Decorate your classes with attributes and let QuickServices handle the rest — no more manual `AddTransient`/`AddScoped`/`AddSingleton` calls.

## Installation

QuickServices targets **.NET 8**, **.NET 9**, and **.NET 10**.

| Package | Description |
|---|---|
| `QuickServices` | Core attribute-based DI registration |
| `QuickServices.Hosting` | `IHostedService` registration support |

## Quick start

```csharp
// Scan a specific assembly
builder.Services.AddQuickServices(typeof(Program).Assembly);

// Or auto-scan the calling assembly and all its referenced projects
builder.Services.AddQuickServices();
```

The parameterless overload automatically discovers all referenced assemblies that use QuickServices — no need to list them manually.

## Attributes

### Standard services

Register a class as its own service type:

```csharp
[TransientService]
public class MyService { }

[ScopedService]
public class MyScopedService { }

[SingletonService]
public class MySingletonService { }
```

### Interface registration

Use the generic variant to register a class as an implementation of an interface:

```csharp
[TransientService<IMyService>]
public class MyService : IMyService { }

[ScopedService<IMyScopedService>]
public class MyScopedService : IMyScopedService { }

[SingletonService<IMySingletonService>]
public class MySingletonService : IMySingletonService { }
```

### Multiple registrations

All attributes support `AllowMultiple = true`, so a single class can be registered under multiple interfaces:

```csharp
[ScopedService<IReader>]
[ScopedService<IWriter>]
public class ReadWriteService : IReader, IWriter { }
```

### Keyed services

Register services with a key for [keyed DI](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) (.NET 8+):

```csharp
[KeyedTransientService<INotifier>("email")]
public class EmailNotifier : INotifier { }

[KeyedScopedService<INotifier>("sms")]
public class SmsNotifier : INotifier { }

[KeyedSingletonService<ICache>("redis")]
public class RedisCache : ICache { }
```

Non-generic variants are also available:

```csharp
[KeyedTransientService("email")]
public class EmailNotifier { }
```

### Hosted services

With the `QuickServices.Hosting` package, register hosted services separately:

```csharp
using QuickServices.Hosting;

// Scan a specific assembly
builder.Services.AddQuickHostedServices(typeof(Program).Assembly);

// Or auto-scan all referenced projects
builder.Services.AddQuickHostedServices();
```

```csharp
[HostedService]
public class MyBackgroundJob : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ...
    }
}
```

The class must implement `IHostedService`, otherwise an `InvalidOperationException` is thrown at startup.

### Registration order

By default, all services are registered with `RegistrationOrder = 1`. When the order matters — for example, when multiple implementations of the same interface are resolved as `IEnumerable<T>` — you can control it explicitly:

```csharp
[ScopedService<IHandler>(registrationOrder: 1)]
public class FirstHandler : IHandler { }

[ScopedService<IHandler>(registrationOrder: 2)]
public class SecondHandler : IHandler { }
```

Services with a lower `RegistrationOrder` are registered first. This ordering is applied globally across all scanned assemblies.

Keyed services support it as well:

```csharp
[KeyedScopedService<IHandler>("key", registrationOrder: 1)]
public class FirstHandler : IHandler { }
```

## Attribute summary

| Attribute | Lifetime | Keyed |
|---|---|---|
| `[TransientService]` / `[TransientService<T>]` | Transient | No |
| `[ScopedService]` / `[ScopedService<T>]` | Scoped | No |
| `[SingletonService]` / `[SingletonService<T>]` | Singleton | No |
| `[KeyedTransientService(key)]` / `[KeyedTransientService<T>(key)]` | Transient | Yes |
| `[KeyedScopedService(key)]` / `[KeyedScopedService<T>(key)]` | Scoped | Yes |
| `[KeyedSingletonService(key)]` / `[KeyedSingletonService<T>(key)]` | Singleton | Yes |
| `[HostedService]` | Singleton | No |

## License

See [LICENSE](LICENSE) for details.
