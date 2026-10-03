using PPEManagement.Context.Repositories;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;
using PPEManagement.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PPEManagement.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="PPECard"/>
/// </summary>
public class PPECardRepository : BaseWriteRepository<PPECard>, IPPECardRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public PPECardRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех карточек СИЗ
    /// </summary>
    Task<IReadOnlyCollection<PPECard>> IPPECardRepository.GetPPECardsAsync(CancellationToken cancellationToken)
        => reader.Read<PPECard>()
            .NotDeletedAt()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.BatchNumber)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение карточки СИЗ по ID
    /// </summary>
    Task<PPECard?> IPPECardRepository.GetPPECardByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<PPECard>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Поиск карточки СИЗ по наименованию и номеру партии
    /// </summary>
    Task<PPECard?> IPPECardRepository.GetPPECardByNameAndBatchAsync(string name, string batchNumber, CancellationToken cancellationToken)
        => reader.Read<PPECard>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && 
                                      x.BatchNumber.ToLower() == batchNumber.ToLower(), cancellationToken);
}