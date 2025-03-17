using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.WebApi.Features.AuditLog;

[ApiController]
[Route("api/audit-events")]
public class AuditEventsController(IDomainEventStore eventStore, ILogger<AuditEventsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Gets audit events filtered by type and date range.
    /// </summary>
    /// <param name="eventType">Type of audit event.</param>
    /// <param name="entityType">Type of entity (example: "Sale", "User").</param>
    /// <param name="startDate">Optional start date.</param>
    /// <param name="endDate">Optional end date.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered audit events.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAuditEvents(
        [FromQuery] AuditEventType eventType,
        [FromQuery] string entityType,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Querying audit events: Type={EventType}, Entity={EntityType}, StartDate={StartDate}, EndDate={EndDate}",
                eventType, entityType, startDate, endDate);

            var entityTypeResolved = ResolveEntityType(entityType);
            var method = typeof(IDomainEventStore)
                .GetMethod(nameof(IDomainEventStore.GetEventsByTypeAsync))!
                .MakeGenericMethod(entityTypeResolved);


            var task = (Task)method.Invoke(eventStore, [eventType, startDate, endDate, cancellationToken])!;
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            var events = resultProperty!.GetValue(task);

            return Ok(events);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching audit events.");
            return StatusCode(500, "Internal error fetching events.");
        }
    }

    /// <summary>
    /// Maps a string to the corresponding entity type.
    /// </summary>
    /// <param name="entityType">Name of the entity (e.g. "sale", "user").</param>
    /// <returns>Type of the corresponding entity, or null if not found.</returns>
    private static Type ResolveEntityType(string entityType)
    {
        return (entityType.ToLower() switch
        {
            "sale" => typeof(Sale),
            "user" => typeof(User),
            _ => null
        })!;
    }
}