using PPEManagement.Entities;

namespace PPEManagement.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="PPEStatement"/>
/// </summary>
public interface IPPEStatementRepository
{
    /// <summary>
    /// Получение всех ведомостей
    /// </summary>
    Task<IReadOnlyCollection<PPEStatement>> GetPPEStatementsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение ведомости по ID (с включением позиций)
    /// </summary>
    Task<PPEStatement?> GetPPEStatementByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Поиск ведомости по регистрационному номеру
    /// </summary>
    Task<PPEStatement?> GetPPEStatementByNumberAsync(string statementNumber, CancellationToken cancellationToken);
}