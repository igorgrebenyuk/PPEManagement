using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="PPEStatementItem"/>
/// </summary>
public interface IPPEStatementItemRepository: IBaseWriteRepository<PPEStatementItem>
{
    /// <summary>
    /// Получение всех строк ведомостей
    /// </summary>
    Task<IReadOnlyCollection<PPEStatementItem>> GetPPEStatementItemsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение строки ведомости по ID
    /// </summary>
    Task<PPEStatementItem?> GetPPEStatementItemByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение всех строк по ID родительской ведомости
    /// </summary>
    Task<IReadOnlyCollection<PPEStatementItem>> GetItemsByStatementIdAsync(Guid statementId, CancellationToken cancellationToken);
}