using PPEManagement.Context.Repositories;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;
using PPEManagement.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PPEManagement.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="PPEStatement"/>
/// </summary>
public class PPEStatementRepository : BaseWriteRepository<PPEStatement>, IPPEStatementRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public PPEStatementRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех ведомостей
    /// </summary>
    Task<IReadOnlyCollection<PPEStatement>> IPPEStatementRepository.GetPPEStatementsAsync(CancellationToken cancellationToken)
        => reader.Read<PPEStatement>()
            .NotDeletedAt()
            .OrderByDescending(x => x.IssueDate)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение ведомости по ID (включая табличную часть)
    /// </summary>
    Task<PPEStatement?> IPPEStatementRepository.GetPPEStatementByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<PPEStatement>()
            .NotDeletedAt()
            .ById(id)
            .Include(x => x.Items.Where(i => i.DeletedAt == null))
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Поиск ведомости по регистрационному номеру
    /// </summary>
    Task<PPEStatement?> IPPEStatementRepository.GetPPEStatementByNumberAsync(string statementNumber, CancellationToken cancellationToken)
        => reader.Read<PPEStatement>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.StatementNumber.ToLower() == statementNumber.ToLower(), cancellationToken);
}