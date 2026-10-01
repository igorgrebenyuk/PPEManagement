using PPEManagement.Common;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Dal.Contracts.Repositories;

namespace PPEManagement.Context.Repositories;

/// <summary>
/// Базовый репозиторий записи сущностей
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class BaseWriteRepository<T> : IBaseWriteRepository.IBaseWriteRepository<T> where T : class, IEntity
{
    protected readonly IWriter Writer;
    protected readonly IDateTimeProvider DateTimeProvider;
    protected readonly IIdentityProvider IdentityProvider;

    public BaseWriteRepository(IDbWriterContext writerContext)
    {
        Writer = writerContext.Writer;
        DateTimeProvider = writerContext.DateTimeProvider;
        IdentityProvider = writerContext.IdentityProvider;
    }

    /// <inheritdoc />
    public virtual void Add(T entity)
    {
        var now = DateTimeProvider.UtcNow;
        var currentUser = IdentityProvider.Name;

        if (entity is IEntityAuditCreated createdAudit)
        {
            createdAudit.CreatedAt = now;
            createdAudit.CreatedBy = currentUser;
        }

        if (entity is IEntityAuditUpdate updatedAudit)
        {
            updatedAudit.UpdatedAt = now;
            updatedAudit.UpdatedBy = currentUser;
        }

        Writer.Add(entity);
    }

    /// <inheritdoc />
    public virtual void Update(T entity)
    {
        if (entity is IEntityAuditUpdate updatedAudit)
        {
            updatedAudit.UpdatedAt = DateTimeProvider.UtcNow;
            updatedAudit.UpdatedBy = IdentityProvider.Name;
        }

        Writer.Update(entity);
    }

    /// <inheritdoc />
    public virtual void Delete(T entity)
    {
        // Мягкое удаление (Soft Delete), если сущность поддерживает IEntityAuditDeletedAt
        if (entity is IEntityAuditDeletedAt deletedAudit)
        {
            deletedAudit.DeletedAt = DateTimeProvider.UtcNow;
            Update(entity);
        }
        else
        {
            Writer.Delete(entity);
        }
    }
}