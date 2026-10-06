using Microsoft.EntityFrameworkCore;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities.Configurations;

namespace PPEManagement.Context;

/// <summary>
/// Контекст базы данных приложения учета выдачи СИЗ (LocalDB / MS SQL Server)
/// </summary>
public class PPEManagementContext : DbContext,
    IReader,
    IWriter,
    IUnitOfWork
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="PPEManagementContext"/>
    /// </summary>
    public PPEManagementContext(DbContextOptions<PPEManagementContext> options)
        : base(options)
    {
        // https://aspnetzero.com
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", isEnabled: true);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntitiesAnchor).Assembly);
    }

    /// <inheritdoc />
    IQueryable<TEntity> IReader.Read<TEntity>()
        => base.Set<TEntity>()
            .AsNoTracking();

    /// <inheritdoc />
    void IWriter.Add<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Added;

    /// <inheritdoc />
    void IWriter.Update<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Modified;

    /// <inheritdoc />
    void IWriter.Delete<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Deleted;

    /// <inheritdoc />
    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        var count = await base.SaveChangesAsync(cancellationToken);
        
        // Отсоединяем все сущности от кэша трекера после сохранения (Stateless подходы / паттерн из репозитория)
        foreach (var entry in base.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return count;
    }
}