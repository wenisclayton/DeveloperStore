using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;

public class AuditEvent<T>(Guid aggregateId, AuditEventType eventType, T data)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AggregateId { get; set; } = aggregateId;
    public AuditEventType EventType { get; set; } = eventType;
    public T Data { get; set; } = data;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Version { get; set; } = 1;
}