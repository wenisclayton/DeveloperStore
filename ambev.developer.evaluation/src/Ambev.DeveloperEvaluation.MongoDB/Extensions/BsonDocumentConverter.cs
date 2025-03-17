using MongoDB.Bson;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.MongoDB.Extensions;

public static class BsonDocumentConverter
{
    public static BsonDocument ToBsonDocument<T>(T entity)
    {
        var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });

        return BsonDocument.Parse(json);
    }
}