using EasyNetQ;
using Microsoft.Extensions.Options;
using Ambev.DeveloperEvaluation.Integration.Interfaces;
using Ambev.DeveloperEvaluation.Integration.Adapters.Messaging.RabbitMQ;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions;


public static class RabbitMQServiceCollectionExtensions
{
    /// <summary>
    /// Registers RabbitMQ integration services using EasyNetQ.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRabbitMQIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

        services.AddSingleton<RabbitMQConnectionFactory>();
        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

        return services;
    }
}