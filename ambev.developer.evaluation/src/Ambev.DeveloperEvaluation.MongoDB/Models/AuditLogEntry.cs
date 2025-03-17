using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.MongoDB.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.MongoDB.Models;

public class AuditLogEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("aggregateId")]
    [BsonRepresentation(BsonType.String)]
    public Guid AggregateId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public AuditEventType EventType { get; set; }

    public BsonDocument Data { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Version { get; set; } = 1;

    public static AuditLogEntry FromDomain<T>(AuditEvent<T> auditEvent)
    {
        return new AuditLogEntry
        {
            AggregateId = auditEvent.AggregateId,
            EventType = auditEvent.EventType,
            Data = BsonDocumentConverter.ToBsonDocument(auditEvent.Data),
            CreatedAt = auditEvent.CreatedAt,
            Version = auditEvent.Version
        };
    }

    //public AuditEvent<T> ToDomain<T>()
    //{
    //    return new AuditEvent<T>(aggregateId: AggregateId, eventType: EventType,
    //        data: BsonSerializer.Deserialize<T>(Data));
    //}

    public T ToDomain<T>()
    {
        var json = Data.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.RelaxedExtendedJson });

        //_logger.LogInformation("Desserializando JSON para {Type}: {Json}", typeof(T).Name, json); // 🔹 Log para depuração

        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}