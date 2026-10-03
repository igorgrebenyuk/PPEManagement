using PPEManagement.Entities;

namespace PPEManagement.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="PPECard"/>
/// </summary>
public interface IPPECardRepository
{
    /// <summary>
    /// Получение всех карточек СИЗ
    /// </summary>
    Task<IReadOnlyCollection<PPECard>> GetPPECardsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение карточки СИЗ по ID
    /// </summary>
    Task<PPECard?> GetPPECardByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Поиск карточки СИЗ по наименованию и номеру партии
    /// </summary>
    Task<PPECard?> GetPPECardByNameAndBatchAsync(string name, string batchNumber, CancellationToken cancellationToken);
}