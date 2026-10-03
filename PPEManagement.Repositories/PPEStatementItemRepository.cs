using PPEManagement.Context.Repositories;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;
using PPEManagement.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PPEManagement.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="PPEStatementItem"/>
/// </summary>
public class PPEStatementItemRepository : BaseWriteRepository<PPEStatementItem>, IPPEStatementItemRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public PPEStatementItemRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех строк ведомостей
    /// </summary>
    Task<IReadOnlyCollection<PPEStatementItem>> IPPEStatementItemRepository.GetPPEStatementItemsAsync(CancellationToken cancellationToken)
        => reader.Read<PPEStatementItem>()
            .NotDeletedAt()
            .OrderBy(x => x.EmployeeFullName)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение строки ведомости по ID
    /// </summary>
    Task<PPEStatementItem?> IPPEStatementItemRepository.GetPPEStatementItemByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<PPEStatementItem>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Получение всех строк по ID родительской ведомости
    /// </summary>
    Task<IReadOnlyCollection<PPEStatementItem>> IPPEStatementItemRepository.GetItemsByStatementIdAsync(Guid statementId, CancellationToken cancellationToken)
        => reader.Read<PPEStatementItem>()
            .NotDeletedAt()
            .Where(x => x.StatementId == statementId)
            .ToReadOnlyCollectionAsync(cancellationToken);
}