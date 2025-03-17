using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.MongoDB.Models;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public class DomainEventStore : IDomainEventStore
{
    private readonly ILogger<DomainEventStore> _logger;
    private const string CollectionName = "AuditLogEntries";
    private readonly IMongoCollection<AuditLogEntry> _eventCollection;

    public DomainEventStore(IMongoClient mongoClient, string databaseName, ILogger<DomainEventStore> logger)
    {
        _logger = logger;
        var database = mongoClient.GetDatabase(databaseName);
        _eventCollection = database.GetCollection<AuditLogEntry>("AuditLogEntries");
    }

    public async Task SaveEventAsync<T>(AuditEvent<T> auditEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var auditLogEntry = AuditLogEntry.FromDomain(auditEvent);
            var bsonDocument = auditLogEntry.ToBsonDocument();
            bsonDocument["Id"] = auditLogEntry.Id.ToString();
            bsonDocument["AggregateId"] = auditLogEntry.AggregateId.ToString();
            await _eventCollection.InsertOneAsync(auditLogEntry, new InsertOneOptions{ BypassDocumentValidation = false }, cancellationToken);


            _logger.LogInformation("Evento salvo: {EventType} - {AggregateId} - Versão {Version}",
                auditEvent.EventType, auditEvent.AggregateId, auditEvent.Version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar evento no MongoDB: {EventType} - {AggregateId}",
                auditEvent.EventType, auditEvent.AggregateId);
            throw;
        }
    }

    public async Task<List<AuditEvent<T>>> GetEventsByTypeAsync<T>(
    AuditEventType eventType,
    DateTime? startDate,
    DateTime? endDate,
    CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<AuditLogEntry>>
    {
        Builders<AuditLogEntry>.Filter.Eq(e => e.EventType, eventType)
    };

        if (startDate.HasValue)
            filters.Add(Builders<AuditLogEntry>.Filter.Gte(e => e.CreatedAt, startDate.Value));

        if (endDate.HasValue)
            filters.Add(Builders<AuditLogEntry>.Filter.Lte(e => e.CreatedAt, endDate.Value));

        var filter = Builders<AuditLogEntry>.Filter.And(filters);

        try
        {
            var auditLogEntries = await _eventCollection
                .Find(filter)
                .SortByDescending(e => e.CreatedAt)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Eventos consultados: {EventType} - {StartDate} a {EndDate} - Total: {Count}",
                eventType, startDate, endDate, auditLogEntries.Count);

            var result = new List<AuditEvent<T>>();

            foreach (var entry in auditLogEntries)
            {
                try
                {
                    var auditEvent = new AuditEvent<T>(
                        aggregateId: entry.AggregateId,
                        eventType: entry.EventType,
                        data: BsonSerializer.Deserialize<T>(entry.Data)
                        );

                    result.Add(auditEvent);
                }
                catch (Exception bsonEx)
                {
                    _logger.LogWarning("Erro ao desserializar usando BsonSerializer: {Message}", bsonEx.Message);

                    try
                    {
                        var json = entry.Data.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.RelaxedExtendedJson });
                        var deserializedData = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        var auditEventFallback = new AuditEvent<T>(
                            aggregateId: entry.AggregateId,
                            eventType: entry.EventType,
                            data: deserializedData!
                        );

                        result.Add(auditEventFallback);
                    }
                    catch (Exception jsonEx)
                    {
                        _logger.LogError(jsonEx, "Falha ao converter evento {EventType} para {Type}", eventType, typeof(T).Name);
                    }
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar eventos no MongoDB: {EventType}", eventType);
            throw;
        }
    }
}