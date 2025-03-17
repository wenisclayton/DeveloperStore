namespace Ambev.DeveloperEvaluation.Integration.Adapters.Messaging.RabbitMQ;

/// <summary>
/// Represents the configuration settings for RabbitMQ.
/// <summary>
/// Represents the configuration settings for EasyNetQ and RabbitMQ.
/// </summary>
public class RabbitMQSettings
{
    /// <summary>
    /// The connection string for RabbitMQ used by EasyNetQ.
    /// Format example: ''host=localhost;username=guest;password=guest;virtualHost=/''
    /// </summary>
    public string ConnectionString { get; set; } = "host=localhost;username=guest;password=guest;virtualHost=/";
}