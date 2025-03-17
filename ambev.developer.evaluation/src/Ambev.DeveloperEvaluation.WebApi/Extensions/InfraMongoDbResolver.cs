using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MongoDB.Repositories;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions;
public static class InfraMongoDbResolver
{
    public static IServiceCollection AddInfrastructureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbConfiguration.ConfigureMongoDbConventions();
        var mongoSettings = configuration.GetSection("MongoDb");

        services.AddSingleton<IMongoClient>(_ =>
        {
            var connectionString = mongoSettings["ConnectionString"];
            return new MongoClient(connectionString);
        });


        services.AddScoped<IDomainEventStore, DomainEventStore>(provider =>
        {
            var mongoClient = provider.GetRequiredService<IMongoClient>();
            var logger = provider.GetRequiredService<ILogger<DomainEventStore>>();
            var databaseName = mongoSettings["DatabaseName"];
            return new DomainEventStore(mongoClient, databaseName!, logger);
        });

        return services;
    }
}

public static class MongoDbConfiguration
{
    /// <summary>
    /// Configura as convenções globais do MongoDB para que os nomes das propriedades sejam interpretados corretamente.
    /// </summary>
    public static void ConfigureMongoDbConventions()
    {
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", conventionPack, _ => true);
    }
}