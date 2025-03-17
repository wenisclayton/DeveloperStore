namespace Ambev.DeveloperEvaluation.Domain.Enums;

public enum AuditEventType : byte
{
    Created = 1,
    Changed = 2,
    Deleted = 3,
    Canceled = 4
}