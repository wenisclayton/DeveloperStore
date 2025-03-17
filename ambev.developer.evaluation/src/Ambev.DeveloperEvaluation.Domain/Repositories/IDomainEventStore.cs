using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IDomainEventStore
{
    Task SaveEventAsync<T>(AuditEvent<T> auditEvent, CancellationToken cancellationToken = default);
    Task<List<AuditEvent<T>>> GetEventsByTypeAsync<T>(AuditEventType eventType, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
}