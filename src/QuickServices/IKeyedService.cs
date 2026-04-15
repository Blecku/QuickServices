namespace QuickServices;

internal interface IKeyedService : IService
{
    object ServiceKey { get; }
}
