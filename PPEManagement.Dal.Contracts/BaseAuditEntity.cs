using PPEManagement.Dal.Contracts.Interfaces;

/// <summary>
/// Базовый класс сущности с полным аудитом
/// </summary>
public abstract class BaseAuditEntity : IEntityWithId, IEntityAuditCreated, IEntityAuditUpdate, IEntityAuditDeletedAt
{
    /// <inheritdoc />
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; }
}