using EasyNetQ;
using EasyNetQ.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Integration.Adapters.Messaging.RabbitMQ;

/// <summary>
/// Factory class to create an instance of IBus using EasyNetQ.
/// Note: With EasyNetQ, connection and channel management is abstracted.
/// </summary>
public class RabbitMQConnectionFactory
{
    private readonly RabbitMQSettings _settings;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQConnectionFactory(RabbitMQSettings settings)
    {
        _settings = settings;
        _serviceProvider = new ServiceCollection()
            .AddSingleton<ITypeNameSerializer>(_ => new CustomTypeNameSerializer())
            .BuildServiceProvider();
    }

    /// <summary>
    /// Creates and returns an instance of IBus.
    /// </summary>
    /// <returns>An IBus instance.</returns>
    public IBus CreateBus() => RabbitHutch.CreateBus(_settings.ConnectionString, s =>
    {
        s.EnableSystemTextJson();
        s.Register<ITypeNameSerializer>(_ => _serviceProvider.GetService<ITypeNameSerializer>()!);
    });
}